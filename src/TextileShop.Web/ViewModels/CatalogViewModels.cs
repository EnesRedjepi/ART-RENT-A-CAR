using TextileShop.Web.Localization;
using TextileShop.Services.Common;
using TextileShop.Services.Models;

namespace TextileShop.Web.ViewModels;

public class ProductFilterViewModel
{
    public string? Search { get; set; }

    public int? CategoryId { get; set; }

    public string? Material { get; set; }

    public string? Color { get; set; }

    public decimal? MinPrice { get; set; }

    public decimal? MaxPrice { get; set; }

    public ProductSort Sort { get; set; } = ProductSort.Newest;

    public int Page { get; set; } = 1;

    public ProductQuery ToQuery(int pageSize) => new()
    {
        Search = Search,
        CategoryId = CategoryId,
        Material = Material,
        Color = Color,
        MinPrice = MinPrice is null ? null : Money.ToEuroLowerBound(MinPrice.Value),
        MaxPrice = MaxPrice is null ? null : Money.ToEuroUpperBound(MaxPrice.Value),
        Sort = Sort,
        Page = Page,
        PageSize = pageSize
    };
}

public class CatalogViewModel
{
    public required PagedResult<ProductListItem> Products { get; init; }

    public required ProductFilterOptions Options { get; init; }

    public required IReadOnlyList<CategoryModel> Categories { get; init; }

    public required ProductFilterViewModel Filter { get; init; }

    public CategoryModel? CurrentCategory { get; init; }
}

public class HomeViewModel
{
    public required IReadOnlyList<CategoryModel> Categories { get; init; }

    public required IReadOnlyList<ProductListItem> NewArrivals { get; init; }
}

public class ProductDetailsViewModel
{
    public required ProductDetails Product { get; init; }

    public required IReadOnlyList<ProductListItem> RelatedProducts { get; init; }
}
