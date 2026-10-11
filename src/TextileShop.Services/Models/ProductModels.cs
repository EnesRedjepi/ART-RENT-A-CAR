using TextileShop.Services.Common;

namespace TextileShop.Services.Models;

public enum ProductSort
{
    Newest,
    PriceAscending,
    PriceDescending,
    NameAscending,
    StockAscending
}

public class ProductQuery
{
    public string? Search { get; set; }

    public int? CategoryId { get; set; }

    public string? Material { get; set; }

    public string? Color { get; set; }

    public decimal? MinPrice { get; set; }

    public decimal? MaxPrice { get; set; }

    public ProductSort Sort { get; set; } = ProductSort.Newest;

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 12;

    public bool IncludeInactive { get; set; }
}

public record ProductListItem(
    int Id,
    string Name,
    string Sku,
    decimal Price,
    decimal WholesalePrice,
    int MinimumWholesaleQuantity,
    int StockQuantity,
    string CategoryName,
    string? Color,
    string? Material,
    string? PrimaryImageFileName,
    bool IsActive)
{
    public bool InStock => StockQuantity > 0;

    public bool IsLowStock => InventoryRules.IsLowStock(StockQuantity);
}

public record ProductImageModel(int Id, string FileName, bool IsPrimary);

/// <summary>A name and description in one language (two-letter code); empty names mean "use the English text".</summary>
public record TextTranslation(string Culture, string Name, string? Description, string? Brand = null, string? Material = null, string? Color = null, string? Size = null);

public record ProductDetails(
    int Id,
    string Name,
    string? Description,
    string Sku,
    decimal Price,
    decimal WholesalePrice,
    int MinimumWholesaleQuantity,
    int StockQuantity,
    int CategoryId,
    string CategoryName,
    string CategorySlug,
    string? Brand,
    string? Material,
    string? Size,
    string? Color,
    bool IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    IReadOnlyList<ProductImageModel> Images,
    string BaseName,
    string? BaseDescription,
    string? BaseBrand,
    string? BaseMaterial,
    string? BaseColor,
    string? BaseSize,
    IReadOnlyList<TextTranslation> Translations)
{
    public bool InStock => StockQuantity > 0;

    public bool IsLowStock => InventoryRules.IsLowStock(StockQuantity);
}

public class ProductInput
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string Sku { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public decimal WholesalePrice { get; set; }

    public int StockQuantity { get; set; }

    public int MinimumWholesaleQuantity { get; set; } = 1;

    public int CategoryId { get; set; }

    public string? Brand { get; set; }

    public string? Material { get; set; }

    public string? Size { get; set; }

    public string? Color { get; set; }

    public bool IsActive { get; set; } = true;

    public IReadOnlyList<TextTranslation> Translations { get; set; } = Array.Empty<TextTranslation>();
}

/// <summary>A filter choice: <see cref="Value"/> is the stored (English) value that is filtered on, <see cref="Label"/> the text shown.</summary>
public record FilterOption(string Value, string Label);

public record ProductFilterOptions(IReadOnlyList<FilterOption> Materials, IReadOnlyList<FilterOption> Colors);

public record ImageUpload(string FileName, long Length, Stream Content);
