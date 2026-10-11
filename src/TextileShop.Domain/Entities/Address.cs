namespace TextileShop.Domain.Entities;

public class Address : BaseEntity
{
    public string? UserId { get; set; }

    public ApplicationUser? User { get; set; }

    public string RecipientName { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string Street { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public string PostalCode { get; set; } = string.Empty;

    public string Country { get; set; } = string.Empty;

    public Address CopyForOrder() => new()
    {
        RecipientName = RecipientName,
        PhoneNumber = PhoneNumber,
        Street = Street,
        City = City,
        PostalCode = PostalCode,
        Country = Country
    };

    public void CopyFrom(Address source)
    {
        RecipientName = source.RecipientName;
        PhoneNumber = source.PhoneNumber;
        Street = source.Street;
        City = source.City;
        PostalCode = source.PostalCode;
        Country = source.Country;
    }
}
