using TextileShop.Services.Models;

namespace TextileShop.Services.Common;

public static class AddressRules
{
    public static IReadOnlyList<string> Validate(AddressData address)
    {
        var errors = new List<string>();

        AddErrorIfBlank(errors, address.RecipientName, "Recipient name is required.");
        AddErrorIfBlank(errors, address.PhoneNumber, "Phone number is required.");
        AddErrorIfBlank(errors, address.Street, "Street is required.");
        AddErrorIfBlank(errors, address.City, "City is required.");
        AddErrorIfBlank(errors, address.PostalCode, "Postal code is required.");
        AddErrorIfBlank(errors, address.Country, "Country is required.");

        return errors;
    }

    private static void AddErrorIfBlank(List<string> errors, string value, string message)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            errors.Add(message);
        }
    }
}
