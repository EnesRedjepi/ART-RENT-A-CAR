using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TextileShop.Domain.Constants;
using TextileShop.Domain.Entities;
using TextileShop.Domain.Enums;
using TextileShop.Infrastructure.Data;
using TextileShop.Services.Abstractions;
using TextileShop.Services.Common;
using TextileShop.Services.Models;

namespace TextileShop.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private const int MaxPageSize = 100;

    private readonly UserManager<ApplicationUser> userManager;
    private readonly SignInManager<ApplicationUser> signInManager;
    private readonly AppDbContext context;
    private readonly TimeProvider timeProvider;
    private readonly ILogger<IdentityService> logger;

    public IdentityService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        AppDbContext context,
        TimeProvider timeProvider,
        ILogger<IdentityService> logger)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.context = context;
        this.timeProvider = timeProvider;
        this.logger = logger;
    }

    public async Task<ServiceResult<string>> RegisterAsync(RegistrationInput input)
    {
        var user = new ApplicationUser
        {
            UserName = input.Email.Trim(),
            Email = input.Email.Trim(),
            FullName = input.FullName.Trim(),
            PhoneNumber = input.PhoneNumber?.Trim(),
            CreatedAt = timeProvider.GetUtcNow().UtcDateTime
        };

        var creation = await userManager.CreateAsync(user, input.Password);
        if (!creation.Succeeded)
        {
            logger.LogError("Registration failed: {Errors}", string.Join(", ", creation.Errors.Select(error => error.Code)));
            return ServiceResult<string>.Failure(creation.Errors.Select(error => error.Description));
        }

        await userManager.AddToRoleAsync(user, Roles.RetailCustomer);
        await signInManager.SignInAsync(user, isPersistent: false);

        logger.LogInformation("User {UserId} registered", user.Id);
        return ServiceResult<string>.Success(user.Id);
    }

    public async Task<ServiceResult<AccountType>> LoginAsync(LoginInput input)
    {
        var user = await userManager.FindByEmailAsync(input.Email.Trim());
        var result = user is null
            ? Microsoft.AspNetCore.Identity.SignInResult.Failed
            : await signInManager.PasswordSignInAsync(user, input.Password, input.RememberMe, lockoutOnFailure: true);

        if (!result.Succeeded)
        {
            logger.LogInformation("Login attempt failed (locked out: {IsLockedOut})", result.IsLockedOut);
            return ServiceResult<AccountType>.Failure(result.IsLockedOut
                ? "This account is temporarily locked. Please try again later."
                : "Invalid email or password.");
        }

        logger.LogInformation("User {UserId} logged in", user!.Id);
        return ServiceResult<AccountType>.Success(await GetAccountTypeAsync(user));
    }

    public async Task LogoutAsync()
    {
        await signInManager.SignOutAsync();
        logger.LogInformation("User logged out");
    }

    public async Task<CustomerModel?> GetCustomerAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        return user is null ? null : ToModel(user, await GetAccountTypeAsync(user));
    }

    public async Task<AccountType> GetAccountTypeAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        return user is null ? AccountType.Retail : await GetAccountTypeAsync(user);
    }

    public async Task<ServiceResult> UpdateProfileAsync(string userId, ProfileInput input)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return ServiceResult.Failure("Account was not found.");
        }

        user.FullName = input.FullName.Trim();
        user.PhoneNumber = input.PhoneNumber?.Trim();

        var update = await userManager.UpdateAsync(user);
        if (!update.Succeeded)
        {
            logger.LogError("Profile update failed for user {UserId}", userId);
            return ServiceResult.Failure(update.Errors.Select(error => error.Description));
        }

        logger.LogInformation("Profile updated for user {UserId}", userId);
        return ServiceResult.Success();
    }

    public async Task<PagedResult<CustomerModel>> SearchCustomersAsync(CustomerQuery query)
    {
        var page = Math.Max(1, query.Page);
        var pageSize = Math.Clamp(query.PageSize, 1, MaxPageSize);
        var users = context.Users.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var pattern = $"%{query.Search.Trim()}%";
            users = users.Where(user => EF.Functions.Like(user.FullName, pattern) || EF.Functions.Like(user.Email!, pattern));
        }

        if (query.AccountType is not null)
        {
            var roleId = await GetRoleIdAsync(Roles.FromAccountType(query.AccountType.Value));
            users = users.Where(user => context.UserRoles.Any(userRole => userRole.UserId == user.Id && userRole.RoleId == roleId));
        }

        var totalCount = await users.CountAsync();
        var pageUsers = await users
            .OrderByDescending(user => user.CreatedAt)
            .ThenBy(user => user.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var accountTypes = await LoadAccountTypesAsync(pageUsers.Select(user => user.Id).ToList());
        var models = pageUsers
            .Select(user => ToModel(user, accountTypes.GetValueOrDefault(user.Id, AccountType.Retail)))
            .ToList();

        return new PagedResult<CustomerModel>(models, page, pageSize, totalCount);
    }

    public async Task<ServiceResult> ChangeAccountTypeAsync(string userId, AccountType accountType)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return ServiceResult.Failure("Account was not found.");
        }

        var currentType = await GetAccountTypeAsync(user);
        if (currentType == AccountType.Administrator || accountType == AccountType.Administrator)
        {
            return ServiceResult.Failure("Administrator accounts cannot be changed from the customer list.");
        }

        if (currentType == accountType)
        {
            return ServiceResult.Success();
        }

        await userManager.RemoveFromRoleAsync(user, Roles.FromAccountType(currentType));
        await userManager.AddToRoleAsync(user, Roles.FromAccountType(accountType));
        await userManager.UpdateSecurityStampAsync(user);

        logger.LogInformation("User {UserId} account type changed from {PreviousType} to {NewType}", userId, currentType, accountType);
        return ServiceResult.Success();
    }

    public async Task<int> CountCustomersAsync()
    {
        var administratorRoleId = await GetRoleIdAsync(Roles.Administrator);
        return await context.Users.CountAsync(user =>
            context.UserRoles.Any(userRole => userRole.UserId == user.Id && userRole.RoleId != administratorRoleId));
    }

    private async Task<string?> GetRoleIdAsync(string roleName)
    {
        return await context.Roles
            .Where(role => role.Name == roleName)
            .Select(role => role.Id)
            .FirstOrDefaultAsync();
    }

    private async Task<Dictionary<string, AccountType>> LoadAccountTypesAsync(List<string> userIds)
    {
        var roleNamesById = await context.Roles.ToDictionaryAsync(role => role.Id, role => role.Name!);
        var assignments = await context.UserRoles
            .Where(userRole => userIds.Contains(userRole.UserId))
            .ToListAsync();

        return assignments
            .GroupBy(assignment => assignment.UserId)
            .ToDictionary(
                group => group.Key,
                group => group.Select(assignment => Roles.ToAccountType(roleNamesById[assignment.RoleId])).Max());
    }

    private async Task<AccountType> GetAccountTypeAsync(ApplicationUser user)
    {
        var roles = await userManager.GetRolesAsync(user);
        return roles.Count == 0 ? AccountType.Retail : roles.Select(Roles.ToAccountType).Max();
    }

    private static CustomerModel ToModel(ApplicationUser user, AccountType accountType) =>
        new(user.Id, user.FullName, user.Email ?? string.Empty, user.PhoneNumber, accountType, user.CreatedAt);
}
