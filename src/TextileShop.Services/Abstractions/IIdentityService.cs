using TextileShop.Domain.Enums;
using TextileShop.Services.Common;
using TextileShop.Services.Models;

namespace TextileShop.Services.Abstractions;

public interface IIdentityService
{
    Task<ServiceResult<string>> RegisterAsync(RegistrationInput input);

    Task<ServiceResult<AccountType>> LoginAsync(LoginInput input);

    Task LogoutAsync();

    Task<CustomerModel?> GetCustomerAsync(string userId);

    Task<AccountType> GetAccountTypeAsync(string userId);

    Task<ServiceResult> UpdateProfileAsync(string userId, ProfileInput input);

    Task<PagedResult<CustomerModel>> SearchCustomersAsync(CustomerQuery query);

    Task<ServiceResult> ChangeAccountTypeAsync(string userId, AccountType accountType);

    Task<int> CountCustomersAsync();
}
