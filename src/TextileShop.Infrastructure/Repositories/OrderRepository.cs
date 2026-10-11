using System.Globalization;
using Microsoft.EntityFrameworkCore;
using TextileShop.Domain.Entities;
using TextileShop.Domain.Enums;
using TextileShop.Infrastructure.Data;
using TextileShop.Services.Abstractions;
using TextileShop.Services.Common;
using TextileShop.Services.Models;

namespace TextileShop.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext context;

    public OrderRepository(AppDbContext context)
    {
        this.context = context;
    }

    public async Task AddAsync(Order order)
    {
        await context.Orders.AddAsync(order);
    }

    public Task<Order?> GetByIdAsync(int id)
    {
        return context.Orders
            .Include(order => order.Customer)
            .Include(order => order.ShippingAddress)
            .Include(order => order.Items)
            .ThenInclude(item => item.Product)
            .ThenInclude(product => product!.Translations)
            .FirstOrDefaultAsync(order => order.Id == id);
    }

    public async Task<PagedResult<Order>> SearchAsync(OrderQuery query)
    {
        var orders = context.Orders.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var pattern = $"%{query.Search.Trim()}%";
            orders = orders.Where(order =>
                EF.Functions.Like(order.OrderNumber, pattern)
                || EF.Functions.Like(order.Customer!.FullName, pattern)
                || EF.Functions.Like(order.Customer!.Email!, pattern));
        }

        if (query.Status is not null)
        {
            orders = orders.Where(order => order.Status == query.Status);
        }

        if (query.PaymentStatus is not null)
        {
            orders = orders.Where(order => order.PaymentStatus == query.PaymentStatus);
        }

        if (query.From is not null)
        {
            var from = query.From.Value.Date;
            orders = orders.Where(order => order.OrderDate >= from);
        }

        if (query.To is not null)
        {
            var exclusiveEnd = query.To.Value.Date.AddDays(1);
            orders = orders.Where(order => order.OrderDate < exclusiveEnd);
        }

        return await ToPagedResultAsync(orders, query.Page, query.PageSize);
    }

    public Task<PagedResult<Order>> GetForCustomerAsync(string customerId, int page, int pageSize)
    {
        var orders = context.Orders.AsNoTracking().Where(order => order.CustomerId == customerId);
        return ToPagedResultAsync(orders, page, pageSize);
    }

    public async Task<IReadOnlyList<Order>> GetRecentAsync(int take)
    {
        return await context.Orders
            .AsNoTracking()
            .Include(order => order.Customer)
            .Include(order => order.Items)
            .OrderByDescending(order => order.OrderDate)
            .Take(take)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<SalesRow>> GetSalesRowsAsync()
    {
        return await context.Orders
            .AsNoTracking()
            .Select(order => new SalesRow(order.OrderDate, order.Status, order.TotalAmount))
            .ToListAsync();
    }

    public async Task<IReadOnlyList<SoldItemRow>> GetSoldItemsAsync()
    {
        var culture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

        return await context.OrderItems
            .AsNoTracking()
            .Where(item => item.Order!.Status != OrderStatus.Cancelled)
            .Select(item => new SoldItemRow(
                item.ProductId,
                item.Product!.Translations
                    .Where(translation => translation.Culture == culture)
                    .Select(translation => translation.Name)
                    .FirstOrDefault() ?? item.ProductName,
                item.Quantity,
                item.UnitPrice))
            .ToListAsync();
    }

    private static async Task<PagedResult<Order>> ToPagedResultAsync(IQueryable<Order> orders, int page, int pageSize)
    {
        var totalCount = await orders.CountAsync();

        var items = await orders
            .Include(order => order.Customer)
            .Include(order => order.Items)
            .OrderByDescending(order => order.OrderDate)
            .ThenByDescending(order => order.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<Order>(items, page, pageSize, totalCount);
    }
}
