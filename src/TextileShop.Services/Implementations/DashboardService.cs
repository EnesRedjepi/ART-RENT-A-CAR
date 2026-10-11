using System.Globalization;
using TextileShop.Domain.Enums;
using TextileShop.Services.Abstractions;
using TextileShop.Services.Common;
using TextileShop.Services.Models;

namespace TextileShop.Services.Implementations;

public class DashboardService : IDashboardService
{
    private const int MonthsInRevenueChart = 6;
    private const int TopProductCount = 5;
    private const int LowStockProductCount = 8;
    private const int RecentOrderCount = 8;

    private readonly IOrderRepository orderRepository;
    private readonly IProductRepository productRepository;
    private readonly IIdentityService identityService;
    private readonly TimeProvider timeProvider;

    public DashboardService(
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        IIdentityService identityService,
        TimeProvider timeProvider)
    {
        this.orderRepository = orderRepository;
        this.productRepository = productRepository;
        this.identityService = identityService;
        this.timeProvider = timeProvider;
    }

    public async Task<DashboardStats> GetStatsAsync()
    {
        var now = timeProvider.GetUtcNow().UtcDateTime;
        var salesRows = await orderRepository.GetSalesRowsAsync();
        var soldItems = await orderRepository.GetSoldItemsAsync();
        var lowStockProducts = await productRepository.GetLowStockAsync(InventoryRules.LowStockThreshold, LowStockProductCount);
        var recentOrders = await orderRepository.GetRecentAsync(RecentOrderCount);

        var revenueRows = salesRows.Where(row => row.Status != OrderStatus.Cancelled).ToList();

        return new DashboardStats(
            revenueRows.Sum(row => row.TotalAmount),
            revenueRows.Where(row => IsInMonth(row.OrderDate, now)).Sum(row => row.TotalAmount),
            salesRows.Count,
            salesRows.Count(row => row.Status == OrderStatus.Pending),
            await productRepository.CountActiveAsync(),
            await identityService.CountCustomersAsync(),
            CountByStatus(salesRows),
            BuildTopProducts(soldItems),
            BuildMonthlyRevenue(revenueRows, now),
            lowStockProducts.Select(product => product.ToListItem()).ToList(),
            recentOrders.Select(order => order.ToListItem()).ToList());
    }

    private static bool IsInMonth(DateTime date, DateTime reference) =>
        date.Year == reference.Year && date.Month == reference.Month;

    private static IReadOnlyDictionary<OrderStatus, int> CountByStatus(IReadOnlyList<SalesRow> rows) =>
        Enum.GetValues<OrderStatus>().ToDictionary(status => status, status => rows.Count(row => row.Status == status));

    private static IReadOnlyList<TopProduct> BuildTopProducts(IReadOnlyList<SoldItemRow> soldItems) =>
        soldItems
            .GroupBy(item => new { item.ProductId, item.ProductName })
            .Select(group => new TopProduct(
                group.Key.ProductName,
                group.Sum(item => item.Quantity),
                group.Sum(item => item.UnitPrice * item.Quantity)))
            .OrderByDescending(product => product.QuantitySold)
            .Take(TopProductCount)
            .ToList();

    private static IReadOnlyList<MonthlyRevenue> BuildMonthlyRevenue(IReadOnlyList<SalesRow> revenueRows, DateTime now)
    {
        var months = Enumerable.Range(0, MonthsInRevenueChart)
            .Select(offset => new DateTime(now.Year, now.Month, 1).AddMonths(-(MonthsInRevenueChart - 1 - offset)));

        return months
            .Select(month => new MonthlyRevenue(
                month.ToString("MMM yyyy", CultureInfo.InvariantCulture),
                revenueRows.Where(row => IsInMonth(row.OrderDate, month)).Sum(row => row.TotalAmount)))
            .ToList();
    }
}
