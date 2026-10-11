using TextileShop.Domain.Entities;
using TextileShop.Services.Common;
using TextileShop.Services.Models;

namespace TextileShop.Services.Abstractions;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(int id);

    Task<PagedResult<Product>> SearchAsync(ProductQuery query);

    Task<bool> SkuExistsAsync(string sku, int? excludedProductId);

    Task<bool> HasOrderItemsAsync(int productId);

    Task<IReadOnlyList<Product>> GetLowStockAsync(int threshold, int take);

    Task<int> CountActiveAsync();

    Task<ProductFilterOptions> GetFilterOptionsAsync();

    Task AddAsync(Product product);

    void Remove(Product product);
}

public interface ICategoryRepository
{
    Task<IReadOnlyList<(Category Category, int ProductCount)>> GetAllWithProductCountsAsync();

    Task<Category?> GetByIdAsync(int id);

    Task<Category?> GetBySlugAsync(string slug);

    Task<bool> NameExistsAsync(string name, int? excludedCategoryId);

    Task<bool> HasProductsAsync(int categoryId);

    Task AddAsync(Category category);

    void Remove(Category category);
}

public interface ICartRepository
{
    Task<IReadOnlyList<CartItem>> GetItemsAsync(string userId);

    Task<CartItem?> GetItemAsync(string userId, int productId);

    Task<int> CountItemsAsync(string userId);

    Task AddAsync(CartItem item);

    void Remove(CartItem item);

    void RemoveRange(IEnumerable<CartItem> items);
}

public interface IOrderRepository
{
    Task AddAsync(Order order);

    Task<Order?> GetByIdAsync(int id);

    Task<PagedResult<Order>> SearchAsync(OrderQuery query);

    Task<PagedResult<Order>> GetForCustomerAsync(string customerId, int page, int pageSize);

    Task<IReadOnlyList<Order>> GetRecentAsync(int take);

    Task<IReadOnlyList<SalesRow>> GetSalesRowsAsync();

    Task<IReadOnlyList<SoldItemRow>> GetSoldItemsAsync();
}

public interface IAddressRepository
{
    Task<Address?> GetDefaultForUserAsync(string userId);

    Task AddAsync(Address address);
}

public interface IUnitOfWork
{
    Task SaveChangesAsync();
}
