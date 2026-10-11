using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace TextileShop.Web.Localization;

public sealed class LocalizedIdentityErrorDescriber : IdentityErrorDescriber
{
    private readonly IStringLocalizer<SharedResource> localizer;

    public LocalizedIdentityErrorDescriber(IStringLocalizer<SharedResource> localizer)
    {
        this.localizer = localizer;
    }

    private IdentityError Error(string code, string key, params object[] arguments) =>
        new() { Code = code, Description = localizer[key, arguments].Value };

    public override IdentityError DefaultError() => Error(nameof(DefaultError), "An unknown failure has occurred.");
    public override IdentityError ConcurrencyFailure() => Error(nameof(ConcurrencyFailure), "Optimistic concurrency failure, object has been modified.");
    public override IdentityError PasswordMismatch() => Error(nameof(PasswordMismatch), "Incorrect password.");
    public override IdentityError InvalidUserName(string? userName) => Error(nameof(InvalidUserName), "User name '{0}' is invalid, can only contain letters or digits.", userName ?? string.Empty);
    public override IdentityError InvalidEmail(string? email) => Error(nameof(InvalidEmail), "Email '{0}' is invalid.", email ?? string.Empty);
    public override IdentityError DuplicateUserName(string userName) => Error(nameof(DuplicateUserName), "User name '{0}' is already taken.", userName);
    public override IdentityError DuplicateEmail(string email) => Error(nameof(DuplicateEmail), "Email '{0}' is already taken.", email);
    public override IdentityError PasswordTooShort(int length) => Error(nameof(PasswordTooShort), "Passwords must be at least {0} characters.", length);
    public override IdentityError PasswordRequiresNonAlphanumeric() => Error(nameof(PasswordRequiresNonAlphanumeric), "Passwords must have at least one non alphanumeric character.");
    public override IdentityError PasswordRequiresDigit() => Error(nameof(PasswordRequiresDigit), "Passwords must have at least one digit ('0'-'9').");
    public override IdentityError PasswordRequiresLower() => Error(nameof(PasswordRequiresLower), "Passwords must have at least one lowercase ('a'-'z').");
    public override IdentityError PasswordRequiresUpper() => Error(nameof(PasswordRequiresUpper), "Passwords must have at least one uppercase ('A'-'Z').");
    public override IdentityError PasswordRequiresUniqueChars(int uniqueChars) => Error(nameof(PasswordRequiresUniqueChars), "Passwords must use at least {0} different characters.", uniqueChars);
}
