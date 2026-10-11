using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using TextileShop.Domain.Enums;
using TextileShop.Services.Common;
using TextileShop.Services.Models;

namespace TextileShop.Web.ViewModels;

/// <summary>One language's name and description in the admin forms; an empty name means "use the English text".</summary>
public abstract class TranslationFormBase
{
    public string Culture { get; set; } = string.Empty;

    public abstract string? Name { get; set; }

    public abstract string? Description { get; set; }

    public virtual TextTranslation ToModel() => new(Culture, Name ?? string.Empty, Description);

    internal static List<T> ForAllLanguages<T>(IEnumerable<TextTranslation> existing) where T : TranslationFormBase, new() =>
        CatalogLanguages.Codes
            .Select(code =>
            {
                var current = existing.FirstOrDefault(translation => string.Equals(translation.Culture, code, StringComparison.OrdinalIgnoreCase));
                return new T { Culture = code, Name = current?.Name, Description = current?.Description };
            })
            .ToList();
}

public class ProductTranslationForm : TranslationFormBase
{
    [StringLength(150)]
    public override string? Name { get; set; }

    [StringLength(4000)]
    public override string? Description { get; set; }

    [StringLength(100)]
    public string? Brand { get; set; }

    [StringLength(100)]
    public string? Material { get; set; }

    [StringLength(100)]
    public string? Color { get; set; }

    [StringLength(100)]
    public string? Size { get; set; }

    public override TextTranslation ToModel() => new(Culture, Name ?? string.Empty, Description, Brand, Material, Color, Size);

    internal static List<ProductTranslationForm> ForAllLanguages(IEnumerable<TextTranslation> existing) =>
        CatalogLanguages.Codes
            .Select(code =>
            {
                var current = existing.FirstOrDefault(translation => string.Equals(translation.Culture, code, StringComparison.OrdinalIgnoreCase));
                return new ProductTranslationForm
                {
                    Culture = code,
                    Name = current?.Name,
                    Description = current?.Description,
                    Brand = current?.Brand,
                    Material = current?.Material,
                    Color = current?.Color,
                    Size = current?.Size
                };
            })
            .ToList();
}

public class CategoryTranslationForm : TranslationFormBase
{
    [StringLength(100)]
    public override string? Name { get; set; }

    [StringLength(500)]
    public override string? Description { get; set; }
}

public class ProductFormViewModel : IValidatableObject
{
    public int Id { get; set; }

    [Required]
    [StringLength(150, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [StringLength(4000)]
    public string? Description { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 2)]
    [Display(Name = "SKU")]
    public string Sku { get; set; } = string.Empty;

    [Range(0.01, 100000)]
    [Display(Name = "Retail price (€)")]
    public decimal Price { get; set; }

    [Range(0.01, 100000)]
    [Display(Name = "Wholesale price (€)")]
    public decimal WholesalePrice { get; set; }

    [Range(0, 1000000)]
    [Display(Name = "Stock quantity")]
    public int StockQuantity { get; set; }

    [Range(1, 100000)]
    [Display(Name = "Minimum wholesale quantity")]
    public int MinimumWholesaleQuantity { get; set; } = 1;

    [Range(1, int.MaxValue, ErrorMessage = "Please select a category.")]
    [Display(Name = "Category")]
    public int CategoryId { get; set; }

    [StringLength(100)]
    public string? Brand { get; set; }

    [StringLength(100)]
    public string? Material { get; set; }

    [StringLength(100)]
    public string? Size { get; set; }

    [StringLength(100)]
    public string? Color { get; set; }

    [Display(Name = "Active (visible in the shop)")]
    public bool IsActive { get; set; } = true;

    [BindNever]
    public IReadOnlyList<SelectListItem> Categories { get; set; } = Array.Empty<SelectListItem>();

    [BindNever]
    public IReadOnlyList<ProductImageModel> Images { get; set; } = Array.Empty<ProductImageModel>();

    public List<ProductTranslationForm> Translations { get; set; } = ProductTranslationForm.ForAllLanguages(Array.Empty<TextTranslation>());

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (WholesalePrice > Price)
        {
            var localizer = validationContext.GetRequiredService<IStringLocalizer<SharedResource>>();
            yield return new ValidationResult(
                localizer["Wholesale price cannot be higher than the retail price."],
                new[] { nameof(WholesalePrice) });
        }
    }

    public ProductInput ToInput() => new()
    {
        Name = Name,
        Description = Description,
        Sku = Sku,
        Price = Price,
        WholesalePrice = WholesalePrice,
        StockQuantity = StockQuantity,
        MinimumWholesaleQuantity = MinimumWholesaleQuantity,
        CategoryId = CategoryId,
        Brand = Brand,
        Material = Material,
        Size = Size,
        Color = Color,
        IsActive = IsActive,
        Translations = Translations.Select(translation => translation.ToModel()).ToList()
    };

    public static ProductFormViewModel From(ProductDetails product) => new()
    {
        Id = product.Id,
        Name = product.BaseName,
        Description = product.BaseDescription,
        Sku = product.Sku,
        Price = product.Price,
        WholesalePrice = product.WholesalePrice,
        StockQuantity = product.StockQuantity,
        MinimumWholesaleQuantity = product.MinimumWholesaleQuantity,
        CategoryId = product.CategoryId,
        Brand = product.BaseBrand,
        Material = product.BaseMaterial,
        Size = product.BaseSize,
        Color = product.BaseColor,
        IsActive = product.IsActive,
        Images = product.Images,
        Translations = ProductTranslationForm.ForAllLanguages(product.Translations)
    };
}

public class CategoryFormViewModel
{
    public int Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    public List<CategoryTranslationForm> Translations { get; set; } = TranslationFormBase.ForAllLanguages<CategoryTranslationForm>(Array.Empty<TextTranslation>());

    public CategoryInput ToInput() => new()
    {
        Name = Name,
        Description = Description,
        Translations = Translations.Select(translation => translation.ToModel()).ToList()
    };

    public static CategoryFormViewModel From(CategoryModel category) => new()
    {
        Id = category.Id,
        Name = category.BaseName,
        Description = category.BaseDescription,
        Translations = TranslationFormBase.ForAllLanguages<CategoryTranslationForm>(category.Translations)
    };
}

public class OrderFilterViewModel
{
    public string? Search { get; set; }

    public OrderStatus? Status { get; set; }

    public PaymentStatus? PaymentStatus { get; set; }

    [DataType(DataType.Date)]
    public DateTime? From { get; set; }

    [DataType(DataType.Date)]
    public DateTime? To { get; set; }

    public int Page { get; set; } = 1;

    public OrderQuery ToQuery() => new()
    {
        Search = Search,
        Status = Status,
        PaymentStatus = PaymentStatus,
        From = From,
        To = To,
        Page = Page
    };
}

public class CustomerFilterViewModel
{
    public string? Search { get; set; }

    public AccountType? AccountType { get; set; }

    public int Page { get; set; } = 1;

    public CustomerQuery ToQuery() => new() { Search = Search, AccountType = AccountType, Page = Page };
}
