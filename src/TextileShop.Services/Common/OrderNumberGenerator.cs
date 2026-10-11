namespace TextileShop.Services.Common;

public static class OrderNumberGenerator
{
    public static string Next(DateTime utcNow) => $"TX-{utcNow:yyyyMMdd}-{Random.Shared.Next(100000, 1000000)}";
}
