using Microsoft.Extensions.Logging;
using TextileShop.Domain.Entities;
using TextileShop.Services.Abstractions;
using TextileShop.Services.Common;
using TextileShop.Services.Models;

namespace TextileShop.Services.Implementations;

public class ProductManagementService : IProductManagementService
{
    private const int MaxPageSize = 100;
    private const int MaxImagesPerProduct = 10;
    private const long MaxImageSizeInBytes = 5 * 1024 * 1024;
    private static readonly string[] AllowedImageExtensions = { ".jpg", ".jpeg", ".png", ".webp" };

    private readonly IProductRepository productRepository;
    private readonly ICategoryRepository categoryRepository;
    private readonly IImageStorage imageStorage;
    private readonly IUnitOfWork unitOfWork;
    private readonly TimeProvider timeProvider;
    private readonly ILogger<ProductManagementService> logger;

    public ProductManagementService(
        IProductRepository productRepository,
        ICategoryRepository categoryRepository,
        IImageStorage imageStorage,
        IUnitOfWork unitOfWork,
        TimeProvider timeProvider,
        ILogger<ProductManagementService> logger)
    {
        this.productRepository = productRepository;
        this.categoryRepository = categoryRepository;
        this.imageStorage = imageStorage;
        this.unitOfWork = unitOfWork;
        this.timeProvider = timeProvider;
        this.logger = logger;
    }

    public async Task<PagedResult<ProductListItem>> SearchAsync(ProductQuery query)
    {
        query.IncludeInactive = true;
        query.Page = Math.Max(1, query.Page);
        query.PageSize = Math.Clamp(query.PageSize, 1, MaxPageSize);

        var products = await productRepository.SearchAsync(query);
        return products.Map(product => product.ToListItem());
    }

    public async Task<ProductDetails?> GetDetailsAsync(int productId)
    {
        var product = await productRepository.GetByIdAsync(productId);
        return product?.ToDetails();
    }

    public async Task<ServiceResult<int>> CreateAsync(ProductInput input)
    {
        var errors = await ValidateAsync(input, null);
        if (errors.Count > 0)
        {
            return ServiceResult<int>.Failure(errors);
        }

        var now = timeProvider.GetUtcNow().UtcDateTime;
        var product = new Product { CreatedAt = now };
        Apply(product, input, now);

        await productRepository.AddAsync(product);
        await unitOfWork.SaveChangesAsync();

        logger.LogInformation("Product {ProductId} created with SKU {Sku}", product.Id, product.Sku);
        return ServiceResult<int>.Success(product.Id);
    }

    public async Task<ServiceResult> UpdateAsync(int productId, ProductInput input)
    {
        var product = await productRepository.GetByIdAsync(productId);
        if (product is null)
        {
            return ServiceResult.Failure("Product was not found.");
        }

        var errors = await ValidateAsync(input, productId);
        if (errors.Count > 0)
        {
            return ServiceResult.Failure(errors);
        }

        Apply(product, input, timeProvider.GetUtcNow().UtcDateTime);
        return await SaveAsync(() => logger.LogInformation("Product {ProductId} updated", productId));
    }

    public async Task<ServiceResult> SetActiveAsync(int productId, bool isActive)
    {
        var product = await productRepository.GetByIdAsync(productId);
        if (product is null)
        {
            return ServiceResult.Failure("Product was not found.");
        }

        product.IsActive = isActive;
        product.UpdatedAt = timeProvider.GetUtcNow().UtcDateTime;
        return await SaveAsync(() => logger.LogInformation("Product {ProductId} active state set to {IsActive}", productId, isActive));
    }

    public async Task<ServiceResult> UpdateStockAsync(int productId, int stockQuantity)
    {
        if (stockQuantity < 0)
        {
            return ServiceResult.Failure("Stock quantity cannot be negative.");
        }

        var product = await productRepository.GetByIdAsync(productId);
        if (product is null)
        {
            return ServiceResult.Failure("Product was not found.");
        }

        product.StockQuantity = stockQuantity;
        product.UpdatedAt = timeProvider.GetUtcNow().UtcDateTime;
        return await SaveAsync(() => logger.LogInformation("Stock for product {ProductId} set to {StockQuantity}", productId, stockQuantity));
    }

    public async Task<ServiceResult> DeleteAsync(int productId)
    {
        var product = await productRepository.GetByIdAsync(productId);
        if (product is null)
        {
            return ServiceResult.Failure("Product was not found.");
        }

        if (await productRepository.HasOrderItemsAsync(productId))
        {
            return ServiceResult.Failure("This product appears in existing orders. Deactivate it instead of deleting it.");
        }

        var imageFileNames = product.Images.Select(image => image.FileName).ToList();
        productRepository.Remove(product);
        await unitOfWork.SaveChangesAsync();

        foreach (var fileName in imageFileNames)
        {
            await imageStorage.DeleteAsync(fileName);
        }

        logger.LogInformation("Product {ProductId} deleted", productId);
        return ServiceResult.Success();
    }

    public async Task<ServiceResult> AddImagesAsync(int productId, IReadOnlyList<ImageUpload> uploads)
    {
        var product = await productRepository.GetByIdAsync(productId);
        if (product is null)
        {
            return ServiceResult.Failure("Product was not found.");
        }

        var errors = ValidateUploads(uploads, product.Images.Count);
        if (errors.Count > 0)
        {
            return ServiceResult.Failure(errors);
        }

        foreach (var upload in uploads)
        {
            var fileName = await imageStorage.SaveAsync(upload.Content, Path.GetExtension(upload.FileName).ToLowerInvariant());
            product.Images.Add(new ProductImage { FileName = fileName, IsPrimary = product.Images.Count == 0 });
        }

        product.UpdatedAt = timeProvider.GetUtcNow().UtcDateTime;
        return await SaveAsync(() => logger.LogInformation("{Count} images added to product {ProductId}", uploads.Count, productId));
    }

    public async Task<ServiceResult> RemoveImageAsync(int productId, int imageId)
    {
        var product = await productRepository.GetByIdAsync(productId);
        var image = product?.Images.FirstOrDefault(candidate => candidate.Id == imageId);
        if (product is null || image is null)
        {
            return ServiceResult.Failure("Image was not found.");
        }

        product.Images.Remove(image);
        if (image.IsPrimary && product.Images.Count > 0)
        {
            product.Images.First().IsPrimary = true;
        }

        var result = await SaveAsync(() => logger.LogInformation("Image {ImageId} removed from product {ProductId}", imageId, productId));
        if (result.Succeeded)
        {
            await imageStorage.DeleteAsync(image.FileName);
        }

        return result;
    }

    public async Task<ServiceResult> SetPrimaryImageAsync(int productId, int imageId)
    {
        var product = await productRepository.GetByIdAsync(productId);
        if (product is null || product.Images.All(image => image.Id != imageId))
        {
            return ServiceResult.Failure("Image was not found.");
        }

        foreach (var image in product.Images)
        {
            image.IsPrimary = image.Id == imageId;
        }

        return await SaveAsync(() => logger.LogInformation("Image {ImageId} set as primary for product {ProductId}", imageId, productId));
    }

    private async Task<ServiceResult> SaveAsync(Action onSuccess)
    {
        try
        {
            await unitOfWork.SaveChangesAsync();
        }
        catch (ConcurrencyConflictException exception)
        {
            logger.LogError(exception, "Concurrent modification detected while saving product data");
            return ServiceResult.Failure("The product was changed by someone else. Please reload and try again.");
        }

        onSuccess();
        return ServiceResult.Success();
    }

    private static void Apply(Product product, ProductInput input, DateTime now)
    {
        product.Name = input.Name.Trim();
        product.Description = input.Description?.Trim();
        product.Sku = input.Sku.Trim().ToUpperInvariant();
        product.Price = input.Price;
        product.WholesalePrice = input.WholesalePrice;
        product.StockQuantity = input.StockQuantity;
        product.MinimumWholesaleQuantity = input.MinimumWholesaleQuantity;
        product.CategoryId = input.CategoryId;
        product.Brand = input.Brand?.Trim();
        product.Material = input.Material?.Trim();
        product.Size = input.Size?.Trim();
        product.Color = input.Color?.Trim();
        product.IsActive = input.IsActive;
        product.UpdatedAt = now;
        TranslationSync.Apply(product.Translations, input.Translations);
    }

    private async Task<IReadOnlyList<string>> ValidateAsync(ProductInput input, int? productId)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(input.Name))
        {
            errors.Add("Product name is required.");
        }

        if (string.IsNullOrWhiteSpace(input.Sku))
        {
            errors.Add("SKU is required.");
        }

        if (input.Price <= 0)
        {
            errors.Add("Retail price must be greater than zero.");
        }

        if (input.WholesalePrice <= 0 || input.WholesalePrice > input.Price)
        {
            errors.Add("Wholesale price must be greater than zero and not higher than the retail price.");
        }

        if (input.StockQuantity < 0)
        {
            errors.Add("Stock quantity cannot be negative.");
        }

        if (input.MinimumWholesaleQuantity < 1)
        {
            errors.Add("Minimum wholesale quantity must be at least 1.");
        }

        if (await categoryRepository.GetByIdAsync(input.CategoryId) is null)
        {
            errors.Add("The selected category does not exist.");
        }

        if (!string.IsNullOrWhiteSpace(input.Sku) && await productRepository.SkuExistsAsync(input.Sku.Trim().ToUpperInvariant(), productId))
        {
            errors.Add("Another product already uses this SKU.");
        }

        return errors;
    }

    private static IReadOnlyList<string> ValidateUploads(IReadOnlyList<ImageUpload> uploads, int existingImageCount)
    {
        var errors = new List<string>();

        if (uploads.Count == 0)
        {
            errors.Add("Please choose at least one image.");
        }

        if (existingImageCount + uploads.Count > MaxImagesPerProduct)
        {
            errors.Add($"A product can have at most {MaxImagesPerProduct} images.");
        }

        foreach (var upload in uploads)
        {
            if (!AllowedImageExtensions.Contains(Path.GetExtension(upload.FileName).ToLowerInvariant()))
            {
                errors.Add($"{upload.FileName}: only JPG, PNG and WEBP images are allowed.");
            }

            if (upload.Length > MaxImageSizeInBytes)
            {
                errors.Add($"{upload.FileName}: images must be 5 MB or smaller.");
            }
        }

        return errors;
    }
}
