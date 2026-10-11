using TextileShop.Services.Abstractions;
using TextileShop.Services.Common;
using TextileShop.Services.Models;

namespace TextileShop.Services.Implementations;

public class CatalogService : ICatalogService
{
    private const int MaxPageSize = 48;

    private readonly IProductRepository productRepository;

    public CatalogService(IProductRepository productRepository)
    {
        this.productRepository = productRepository;
    }

    public async Task<PagedResult<ProductListItem>> SearchAsync(ProductQuery query)
    {
        query.IncludeInactive = false;
        query.Page = Math.Max(1, query.Page);
        query.PageSize = Math.Clamp(query.PageSize, 1, MaxPageSize);

        var products = await productRepository.SearchAsync(query);
        return products.Map(product => product.ToListItem());
    }

    public async Task<ProductDetails?> GetDetailsAsync(int productId)
    {
        var product = await productRepository.GetByIdAsync(productId);
        return product is { IsActive: true } ? product.ToDetails() : null;
    }

    public Task<ProductFilterOptions> GetFilterOptionsAsync() => productRepository.GetFilterOptionsAsync();
}
