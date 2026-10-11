namespace TextileShop.Services.Common;

public static class InventoryRules
{
    public const int LowStockThreshold = 5;

    public const int MaxQuantityPerLine = 1000;

    public static bool IsLowStock(int stockQuantity) => stockQuantity <= LowStockThreshold;
}
