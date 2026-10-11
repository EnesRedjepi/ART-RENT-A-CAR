using TextileShop.Domain.Entities;
using TextileShop.Domain.Enums;
using TextileShop.Services.Abstractions;

namespace TextileShop.Services.Implementations;

public class PricingService : IPricingService
{
    public PriceQuote GetPrice(Product product, int quantity, AccountType accountType)
    {
        var qualifiesForWholesale = accountType == AccountType.Wholesale
            && quantity >= product.MinimumWholesaleQuantity;

        return qualifiesForWholesale
            ? new PriceQuote(product.WholesalePrice, true)
            : new PriceQuote(product.Price, false);
    }
}
