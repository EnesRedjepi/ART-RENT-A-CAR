using System.Globalization;
using Microsoft.EntityFrameworkCore;
using TextileShop.Domain.Entities;
using TextileShop.Infrastructure.Data;
using TextileShop.Services.Abstractions;
using TextileShop.Services.Common;
using TextileShop.Services.Models;

namespace TextileShop.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext context;

    public ProductRepository(AppDbContext context)
    {
        this.context = context;
    }

    public Task<Product?> GetByIdAsync(int id)
    {
        return context.Products
            .Include(product => product.Category)
            .ThenInclude(category => category!.Translations)
            .Include(product => product.Images)
            .Include(product => product.Translations)
            .FirstOrDefaultAsync(product => product.Id == id);
    }

    public async Task<PagedResult<Product>> SearchAsync(ProductQuery query)
    {
        var products = ApplyFilters(context.Products.AsNoTracking(), query);
        var totalCount = await products.CountAsync();

        if (query.Sort == ProductSort.NameAscending)
        {
            return new PagedResult<Product>(await GetPageSortedByNameAsync(products, query), query.Page, query.PageSize, totalCount);
        }

        var items = await WithDisplayData(ApplySorting(products, query.Sort))
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();

        return new PagedResult<Product>(items, query.Page, query.PageSize, totalCount);
    }

    private static IQueryable<Product> WithDisplayData(IQueryable<Product> products) => products
        .Include(product => product.Category)
        .ThenInclude(category => category!.Translations)
        .Include(product => product.Images)
        .Include(product => product.Translations);

    /// <summary>
    /// Sorting by name uses the alphabet of the current language (the database would compare raw character codes,
    /// which misplaces letters such as Macedonian Ќ or Albanian Ç), so the page is picked in memory.
    /// </summary>
    private async Task<List<Product>> GetPageSortedByNameAsync(IQueryable<Product> products, ProductQuery query)
    {
        var culture = CultureInfo.CurrentUICulture;
        var code = culture.TwoLetterISOLanguageName;

        var names = await products
            .Select(product => new
            {
                product.Id,
                Name = product.Translations
                    .Where(translation => translation.Culture == code)
                    .Select(translation => translation.Name)
                    .FirstOrDefault() ?? product.Name
            })
            .ToListAsync();

        var comparer = StringComparer.Create(culture, ignoreCase: true);
        var pageIds = names
            .OrderBy(row => row.Name, comparer)
            .ThenBy(row => row.Id)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(row => row.Id)
            .ToList();

        var loaded = await WithDisplayData(products.Where(product => pageIds.Contains(product.Id))).ToListAsync();
        return pageIds.Select(id => loaded.First(product => product.Id == id)).ToList();
    }

    public Task<bool> SkuExistsAsync(string sku, int? excludedProductId)
    {
        return context.Products.AnyAsync(product => product.Sku == sku && product.Id != excludedProductId);
    }

    public Task<bool> HasOrderItemsAsync(int productId)
    {
        return context.OrderItems.AnyAsync(item => item.ProductId == productId);
    }

    public async Task<IReadOnlyList<Product>> GetLowStockAsync(int threshold, int take)
    {
        return await context.Products
            .AsNoTracking()
            .Include(product => product.Category)
            .ThenInclude(category => category!.Translations)
            .Include(product => product.Images)
            .Include(product => product.Translations)
            .Where(product => product.IsActive && product.StockQuantity <= threshold)
            .OrderBy(product => product.StockQuantity)
            .Take(take)
            .ToListAsync();
    }

    public Task<int> CountActiveAsync()
    {
        return context.Products.CountAsync(product => product.IsActive);
    }

    public async Task<ProductFilterOptions> GetFilterOptionsAsync()
    {
        var culture = CultureInfo.CurrentUICulture;
        var code = culture.TwoLetterISOLanguageName;
        var activeProducts = context.Products.AsNoTracking().Where(product => product.IsActive);

        var materialRows = await activeProducts
            .Where(product => product.Material != null && product.Material != string.Empty)
            .Select(product => new
            {
                Value = product.Material!,
                Label = product.Translations.Where(translation => translation.Culture == code).Select(translation => translation.Material).FirstOrDefault()
            })
            .ToListAsync();

        var colorRows = await activeProducts
            .Where(product => product.Color != null && product.Color != string.Empty)
            .Select(product => new
            {
                Value = product.Color!,
                Label = product.Translations.Where(translation => translation.Culture == code).Select(translation => translation.Color).FirstOrDefault()
            })
            .ToListAsync();

        return new ProductFilterOptions(ToOptions(materialRows.Select(row => (row.Value, row.Label)), culture), ToOptions(colorRows.Select(row => (row.Value, row.Label)), culture));
    }

    /// <summary>One option per stored value, labelled with its translation when there is one, in the alphabetical order of the language.</summary>
    private static IReadOnlyList<FilterOption> ToOptions(IEnumerable<(string Value, string? Label)> rows, CultureInfo culture) => rows
        .GroupBy(row => row.Value)
        .Select(group => new FilterOption(
            group.Key,
            group.Select(row => row.Label).FirstOrDefault(label => !string.IsNullOrWhiteSpace(label)) ?? group.Key))
        .OrderBy(option => option.Label, StringComparer.Create(culture, ignoreCase: true))
        .ToList();

    public async Task AddAsync(Product product)
    {
        await context.Products.AddAsync(product);
    }

    public void Remove(Product product)
    {
        context.Products.Remove(product);
    }

    private static IQueryable<Product> ApplyFilters(IQueryable<Product> products, ProductQuery query)
    {
        if (!query.IncludeInactive)
        {
            products = products.Where(product => product.IsActive);
        }

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var pattern = $"%{query.Search.Trim()}%";
            products = products.Where(product =>
                EF.Functions.Like(product.Name, pattern)
                || product.Translations.Any(translation => EF.Functions.Like(translation.Name, pattern))
                || EF.Functions.Like(product.Sku, pattern)
                || EF.Functions.Like(product.Description!, pattern)
                || product.Translations.Any(translation => EF.Functions.Like(translation.Description!, pattern))
                || EF.Functions.Like(product.Brand!, pattern)
                || product.Translations.Any(translation => EF.Functions.Like(translation.Brand!, pattern)));
        }

        if (query.CategoryId is not null)
        {
            products = products.Where(product => product.CategoryId == query.CategoryId);
        }

        if (!string.IsNullOrWhiteSpace(query.Material))
        {
            products = products.Where(product => product.Material == query.Material);
        }

        if (!string.IsNullOrWhiteSpace(query.Color))
        {
            products = products.Where(product => product.Color == query.Color);
        }

        if (query.MinPrice is not null)
        {
            products = products.Where(product => product.Price >= query.MinPrice);
        }

        if (query.MaxPrice is not null)
        {
            products = products.Where(product => product.Price <= query.MaxPrice);
        }

        return products;
    }

    private static IQueryable<Product> ApplySorting(IQueryable<Product> products, ProductSort sort)
    {
        var culture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

        return sort switch
        {
            ProductSort.PriceAscending => products.OrderBy(product => product.Price).ThenBy(product => product.Id),
            ProductSort.PriceDescending => products.OrderByDescending(product => product.Price).ThenBy(product => product.Id),
            ProductSort.NameAscending => products
                .OrderBy(product => product.Translations
                    .Where(translation => translation.Culture == culture)
                    .Select(translation => translation.Name)
                    .FirstOrDefault() ?? product.Name)
                .ThenBy(product => product.Id),
            ProductSort.StockAscending => products.OrderBy(product => product.StockQuantity).ThenBy(product => product.Id),
            _ => products.OrderByDescending(product => product.CreatedAt).ThenBy(product => product.Id)
        };
    }
}
