using TextileShop.Domain.Entities;
using TextileShop.Services.Abstractions;
using TextileShop.Services.Common;
using TextileShop.Services.Models;

namespace TextileShop.Services.Implementations;

public class CartService : ICartService
{
    private readonly ICartRepository cartRepository;
    private readonly IProductRepository productRepository;
    private readonly IIdentityService identityService;
    private readonly IPricingService pricingService;
    private readonly IUnitOfWork unitOfWork;

    public CartService(
        ICartRepository cartRepository,
        IProductRepository productRepository,
        IIdentityService identityService,
        IPricingService pricingService,
        IUnitOfWork unitOfWork)
    {
        this.cartRepository = cartRepository;
        this.productRepository = productRepository;
        this.identityService = identityService;
        this.pricingService = pricingService;
        this.unitOfWork = unitOfWork;
    }

    public async Task<CartModel> GetCartAsync(string userId)
    {
        var items = await cartRepository.GetItemsAsync(userId);
        var accountType = await identityService.GetAccountTypeAsync(userId);

        var lines = items
            .Select(item =>
            {
                var product = item.Product!;
                var price = pricingService.GetPrice(product, item.Quantity, accountType);
                return new CartLine(
                    product.Id,
                    product.DisplayName(),
                    product.Sku,
                    product.PrimaryImageFileName(),
                    item.Quantity,
                    price.UnitPrice,
                    price.IsWholesalePrice,
                    product.StockQuantity,
                    product.IsActive && product.StockQuantity > 0,
                    product.MinimumWholesaleQuantity);
            })
            .ToList();

        return new CartModel(lines);
    }

    public Task<int> GetItemCountAsync(string userId) => cartRepository.CountItemsAsync(userId);

    public async Task<ServiceResult> AddAsync(string userId, int productId, int quantity)
    {
        var existingItem = await cartRepository.GetItemAsync(userId, productId);
        var requestedQuantity = (existingItem?.Quantity ?? 0) + quantity;

        var validation = await ValidateQuantityAsync(productId, requestedQuantity);
        if (!validation.Succeeded)
        {
            return validation;
        }

        if (existingItem is null)
        {
            await cartRepository.AddAsync(new CartItem { UserId = userId, ProductId = productId, Quantity = quantity });
        }
        else
        {
            existingItem.Quantity = requestedQuantity;
        }

        await unitOfWork.SaveChangesAsync();
        return ServiceResult.Success();
    }

    public async Task<ServiceResult> SetQuantityAsync(string userId, int productId, int quantity)
    {
        if (quantity <= 0)
        {
            return await RemoveAsync(userId, productId);
        }

        var item = await cartRepository.GetItemAsync(userId, productId);
        if (item is null)
        {
            return ServiceResult.Failure("The item is not in your cart.");
        }

        var validation = await ValidateQuantityAsync(productId, quantity);
        if (!validation.Succeeded)
        {
            return validation;
        }

        item.Quantity = quantity;
        await unitOfWork.SaveChangesAsync();
        return ServiceResult.Success();
    }

    public async Task<ServiceResult> RemoveAsync(string userId, int productId)
    {
        var item = await cartRepository.GetItemAsync(userId, productId);
        if (item is not null)
        {
            cartRepository.Remove(item);
            await unitOfWork.SaveChangesAsync();
        }

        return ServiceResult.Success();
    }

    private async Task<ServiceResult> ValidateQuantityAsync(int productId, int quantity)
    {
        if (quantity < 1 || quantity > InventoryRules.MaxQuantityPerLine)
        {
            return ServiceResult.Failure($"Quantity must be between 1 and {InventoryRules.MaxQuantityPerLine}.");
        }

        var product = await productRepository.GetByIdAsync(productId);
        if (product is null || !product.IsActive)
        {
            return ServiceResult.Failure("This product is no longer available.");
        }

        if (!product.HasStockFor(quantity))
        {
            return ServiceResult.Failure($"Only {product.StockQuantity} units of {product.DisplayName()} are in stock.");
        }

        return ServiceResult.Success();
    }
}
