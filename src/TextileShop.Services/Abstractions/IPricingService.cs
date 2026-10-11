using TextileShop.Domain.Entities;
using TextileShop.Domain.Enums;

namespace TextileShop.Services.Abstractions;

public record PriceQuote(decimal UnitPrice, bool IsWholesalePrice);

public interface IPricingService
{
    PriceQuote GetPrice(Product product, int quantity, AccountType accountType);
}
