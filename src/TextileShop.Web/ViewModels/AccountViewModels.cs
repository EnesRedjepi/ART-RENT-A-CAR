using System.ComponentModel.DataAnnotations;
using TextileShop.Domain.Enums;
using TextileShop.Services.Models;

namespace TextileShop.Web.ViewModels;

public class LoginViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Remember me")]
    public bool RememberMe { get; set; }

    public LoginInput ToInput() => new() { Email = Email, Password = Password, RememberMe = RememberMe };
}

public class RegisterViewModel
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    [Display(Name = "Full name")]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(150)]
    public string Email { get; set; } = string.Empty;

    [Phone]
    [StringLength(30)]
    [Display(Name = "Phone number")]
    public string? PhoneNumber { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 8)]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "The passwords do not match.")]
    [Display(Name = "Confirm password")]
    public string ConfirmPassword { get; set; } = string.Empty;

    public RegistrationInput ToInput() => new()
    {
        FullName = FullName,
        Email = Email,
        PhoneNumber = PhoneNumber,
        Password = Password
    };
}

public class ProfileViewModel
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    [Display(Name = "Full name")]
    public string FullName { get; set; } = string.Empty;

    [Phone]
    [StringLength(30)]
    [Display(Name = "Phone number")]
    public string? PhoneNumber { get; set; }

    public string Email { get; set; } = string.Empty;

    public AccountType AccountType { get; set; }

    public ProfileInput ToInput() => new() { FullName = FullName, PhoneNumber = PhoneNumber };

    public static ProfileViewModel From(CustomerModel customer) => new()
    {
        FullName = customer.FullName,
        PhoneNumber = customer.PhoneNumber,
        Email = customer.Email,
        AccountType = customer.AccountType
    };
}

public class AddressViewModel
{
    [Required]
    [StringLength(100)]
    [Display(Name = "Recipient name")]
    public string RecipientName { get; set; } = string.Empty;

    [Required]
    [Phone]
    [StringLength(30)]
    [Display(Name = "Phone number")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    [Display(Name = "Street and number")]
    public string Street { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string City { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    [Display(Name = "Postal code")]
    public string PostalCode { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Country { get; set; } = string.Empty;

    public AddressData ToData() => new(RecipientName, PhoneNumber, Street, City, PostalCode, Country);

    public static AddressViewModel From(AddressData data) => new()
    {
        RecipientName = data.RecipientName,
        PhoneNumber = data.PhoneNumber,
        Street = data.Street,
        City = data.City,
        PostalCode = data.PostalCode,
        Country = data.Country
    };
}
