using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace TextileShop.Infrastructure.Data;

public class DecimalToCentsConverter : ValueConverter<decimal, long>
{
    public DecimalToCentsConverter()
        : base(
            amount => (long)Math.Round(amount * 100m, 0, MidpointRounding.AwayFromZero),
            cents => cents / 100m)
    {
    }
}
