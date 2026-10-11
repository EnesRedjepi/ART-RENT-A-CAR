using Microsoft.EntityFrameworkCore;
using TextileShop.Domain.Entities;
using TextileShop.Infrastructure.Data;
using TextileShop.Services.Abstractions;

namespace TextileShop.Infrastructure.Repositories;

public class CartRepository : ICartRepository
{
    private readonly AppDbContext context;

    public CartRepository(AppDbContext context)
    {
        this.context = context;
    }

    public async Task<IReadOnlyList<CartItem>> GetItemsAsync(string userId)
    {
        return await context.CartItems
            .Include(item => item.Product)
            .ThenInclude(product => product!.Images)
            .Include(item => item.Product)
            .ThenInclude(product => product!.Translations)
            .Where(item => item.UserId == userId)
            .OrderBy(item => item.Id)
            .ToListAsync();
    }

    public Task<CartItem?> GetItemAsync(string userId, int productId)
    {
        return context.CartItems.FirstOrDefaultAsync(item => item.UserId == userId && item.ProductId == productId);
    }

    public Task<int> CountItemsAsync(string userId)
    {
        return context.CartItems.Where(item => item.UserId == userId).SumAsync(item => item.Quantity);
    }

    public async Task AddAsync(CartItem item)
    {
        await context.CartItems.AddAsync(item);
    }

    public void Remove(CartItem item)
    {
        context.CartItems.Remove(item);
    }

    public void RemoveRange(IEnumerable<CartItem> items)
    {
        context.CartItems.RemoveRange(items);
    }
}
