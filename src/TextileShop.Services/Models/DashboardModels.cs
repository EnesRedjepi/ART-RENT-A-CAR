using TextileShop.Domain.Enums;

namespace TextileShop.Services.Models;

public record SalesRow(DateTime OrderDate, OrderStatus Status, decimal TotalAmount);

public record SoldItemRow(int ProductId, string ProductName, int Quantity, decimal UnitPrice);

public record TopProduct(string ProductName, int QuantitySold, decimal Revenue);

public record MonthlyRevenue(string Label, decimal Amount);

public record DashboardStats(
    decimal TotalRevenue,
    decimal CurrentMonthRevenue,
    int TotalOrders,
    int PendingOrders,
    int ActiveProducts,
    int CustomerCount,
    IReadOnlyDictionary<OrderStatus, int> OrdersByStatus,
    IReadOnlyList<TopProduct> TopProducts,
    IReadOnlyList<MonthlyRevenue> RevenueByMonth,
    IReadOnlyList<ProductListItem> LowStockProducts,
    IReadOnlyList<OrderListItem> RecentOrders);
