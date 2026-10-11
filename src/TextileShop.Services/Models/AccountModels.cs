using TextileShop.Domain.Enums;

namespace TextileShop.Services.Models;

public class RegistrationInput
{
    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? PhoneNumber { get; set; }

    public string Password { get; set; } = string.Empty;
}

public class LoginInput
{
    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public bool RememberMe { get; set; }
}

public class ProfileInput
{
    public string FullName { get; set; } = string.Empty;

    public string? PhoneNumber { get; set; }
}

public class CustomerQuery
{
    public string? Search { get; set; }

    public AccountType? AccountType { get; set; }

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 15;
}

public record CustomerModel(
    string Id,
    string FullName,
    string Email,
    string? PhoneNumber,
    AccountType AccountType,
    DateTime CreatedAt);
