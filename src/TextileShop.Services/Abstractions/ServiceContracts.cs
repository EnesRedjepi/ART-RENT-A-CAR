using TextileShop.Domain.Enums;
using TextileShop.Services.Common;
using TextileShop.Services.Models;

namespace TextileShop.Services.Abstractions;

public interface ICatalogService
{
    Task<PagedResult<ProductListItem>> SearchAsync(ProductQuery query);

    Task<ProductDetails?> GetDetailsAsync(int productId);

    Task<ProductFilterOptions> GetFilterOptionsAsync();
}

public interface IProductManagementService
{
    Task<PagedResult<ProductListItem>> SearchAsync(ProductQuery query);

    Task<ProductDetails?> GetDetailsAsync(int productId);

    Task<ServiceResult<int>> CreateAsync(ProductInput input);

    Task<ServiceResult> UpdateAsync(int productId, ProductInput input);

    Task<ServiceResult> SetActiveAsync(int productId, bool isActive);

    Task<ServiceResult> UpdateStockAsync(int productId, int stockQuantity);

    Task<ServiceResult> DeleteAsync(int productId);

    Task<ServiceResult> AddImagesAsync(int productId, IReadOnlyList<ImageUpload> uploads);

    Task<ServiceResult> RemoveImageAsync(int productId, int imageId);

    Task<ServiceResult> SetPrimaryImageAsync(int productId, int imageId);
}

public interface ICategoryService
{
    Task<IReadOnlyList<CategoryModel>> GetAllAsync();

    Task<CategoryModel?> GetByIdAsync(int id);

    Task<CategoryModel?> GetBySlugAsync(string slug);

    Task<ServiceResult> CreateAsync(CategoryInput input);

    Task<ServiceResult> UpdateAsync(int id, CategoryInput input);

    Task<ServiceResult> DeleteAsync(int id);
}

public interface ICartService
{
    Task<CartModel> GetCartAsync(string userId);

    Task<int> GetItemCountAsync(string userId);

    Task<ServiceResult> AddAsync(string userId, int productId, int quantity);

    Task<ServiceResult> SetQuantityAsync(string userId, int productId, int quantity);

    Task<ServiceResult> RemoveAsync(string userId, int productId);
}

public interface IOrderService
{
    Task<ServiceResult<int>> PlaceOrderAsync(string userId, CheckoutInput input);

    Task<PagedResult<OrderListItem>> GetCustomerOrdersAsync(string userId, int page);

    Task<OrderDetails?> GetCustomerOrderAsync(string userId, int orderId);

    Task<ServiceResult> CancelCustomerOrderAsync(string userId, int orderId);

    Task<PagedResult<OrderListItem>> SearchAsync(OrderQuery query);

    Task<OrderDetails?> GetAsync(int orderId);

    Task<ServiceResult> UpdateStatusAsync(int orderId, OrderStatus status);

    Task<ServiceResult> UpdatePaymentStatusAsync(int orderId, PaymentStatus status);
}

public interface IAddressService
{
    Task<AddressData?> GetDeliveryAddressAsync(string userId);

    Task<ServiceResult> SaveDeliveryAddressAsync(string userId, AddressData data);
}

public interface IDashboardService
{
    Task<DashboardStats> GetStatsAsync();
}
