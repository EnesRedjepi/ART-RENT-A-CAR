using System.Globalization;
using TextileShop.Domain.Entities;
using TextileShop.Services.Common;
using TextileShop.Services.Models;

namespace TextileShop.Services.Implementations;

internal static class Mapping
{
    private static string CurrentCulture => CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

    private static bool IsFor(string culture) => string.Equals(culture, CurrentCulture, StringComparison.OrdinalIgnoreCase);

    // Names and descriptions in the current UI language; English (the stored value) when there is no translation.
    public static string DisplayName(this Product product) =>
        product.Translations.FirstOrDefault(translation => IsFor(translation.Culture) && translation.Name.Length > 0)?.Name ?? product.Name;

    public static string? DisplayDescription(this Product product) =>
        product.Translations.FirstOrDefault(translation => IsFor(translation.Culture) && !string.IsNullOrWhiteSpace(translation.Description))?.Description
        ?? product.Description;

    public static string? DisplayBrand(this Product product) => product.Pick(translation => translation.Brand, product.Brand);

    public static string? DisplayMaterial(this Product product) => product.Pick(translation => translation.Material, product.Material);

    public static string? DisplayColor(this Product product) => product.Pick(translation => translation.Color, product.Color);

    public static string? DisplaySize(this Product product) => product.Pick(translation => translation.Size, product.Size);

    private static string? Pick(this Product product, Func<ProductTranslation, string?> field, string? english) =>
        product.Translations.Where(translation => IsFor(translation.Culture)).Select(field).FirstOrDefault(value => !string.IsNullOrWhiteSpace(value))
        ?? english;

    public static string DisplayName(this Category category) =>
        category.Translations.FirstOrDefault(translation => IsFor(translation.Culture) && translation.Name.Length > 0)?.Name ?? category.Name;

    public static string? DisplayDescription(this Category category) =>
        category.Translations.FirstOrDefault(translation => IsFor(translation.Culture) && !string.IsNullOrWhiteSpace(translation.Description))?.Description
        ?? category.Description;

    /// <summary>The name an order line is shown with: the product's translation if it has one, otherwise the name recorded when ordered.</summary>
    public static string DisplayName(this OrderItem item) =>
        item.Product?.Translations.FirstOrDefault(translation => IsFor(translation.Culture) && translation.Name.Length > 0)?.Name ?? item.ProductName;

    public static IReadOnlyList<TextTranslation> ToModels(this IEnumerable<ProductTranslation> translations) =>
        translations.Select(translation => new TextTranslation(
            translation.Culture, translation.Name, translation.Description, translation.Brand, translation.Material, translation.Color, translation.Size)).ToList();

    public static IReadOnlyList<TextTranslation> ToModels(this IEnumerable<CategoryTranslation> translations) =>
        translations.Select(translation => new TextTranslation(translation.Culture, translation.Name, translation.Description)).ToList();

    public static string? PrimaryImageFileName(this Product product) =>
        (product.Images.FirstOrDefault(image => image.IsPrimary) ?? product.Images.FirstOrDefault())?.FileName;

    public static ProductListItem ToListItem(this Product product) => new(
        product.Id,
        product.DisplayName(),
        product.Sku,
        product.Price,
        product.WholesalePrice,
        product.MinimumWholesaleQuantity,
        product.StockQuantity,
        product.Category?.DisplayName() ?? string.Empty,
        product.DisplayColor(),
        product.DisplayMaterial(),
        product.PrimaryImageFileName(),
        product.IsActive);

    public static ProductDetails ToDetails(this Product product) => new(
        product.Id,
        product.DisplayName(),
        product.DisplayDescription(),
        product.Sku,
        product.Price,
        product.WholesalePrice,
        product.MinimumWholesaleQuantity,
        product.StockQuantity,
        product.CategoryId,
        product.Category?.DisplayName() ?? string.Empty,
        product.Category?.Slug ?? string.Empty,
        product.DisplayBrand(),
        product.DisplayMaterial(),
        product.DisplaySize(),
        product.DisplayColor(),
        product.IsActive,
        product.CreatedAt,
        product.UpdatedAt,
        product.Images
            .OrderByDescending(image => image.IsPrimary)
            .ThenBy(image => image.Id)
            .Select(image => new ProductImageModel(image.Id, image.FileName, image.IsPrimary))
            .ToList(),
        product.Name,
        product.Description,
        product.Brand,
        product.Material,
        product.Color,
        product.Size,
        product.Translations.ToModels());

    public static AddressData ToData(this Address address) => new(
        address.RecipientName,
        address.PhoneNumber,
        address.Street,
        address.City,
        address.PostalCode,
        address.Country);

    public static Address ToEntity(this AddressData data) => new()
    {
        RecipientName = data.RecipientName.Trim(),
        PhoneNumber = data.PhoneNumber.Trim(),
        Street = data.Street.Trim(),
        City = data.City.Trim(),
        PostalCode = data.PostalCode.Trim(),
        Country = data.Country.Trim()
    };

    public static OrderListItem ToListItem(this Order order) => new(
        order.Id,
        order.OrderNumber,
        order.OrderDate,
        order.Status,
        order.PaymentStatus,
        order.Customer?.FullName ?? string.Empty,
        order.Customer?.Email ?? string.Empty,
        order.Items.Sum(item => item.Quantity),
        order.TotalAmount);

    public static OrderDetails ToDetails(this Order order) => new(
        order.Id,
        order.OrderNumber,
        order.OrderDate,
        order.Status,
        order.PaymentStatus,
        order.Customer?.FullName ?? string.Empty,
        order.Customer?.Email ?? string.Empty,
        order.ShippingAddress!.ToData(),
        order.Notes,
        order.Items
            .Select(item => new OrderItemModel(
                item.ProductId,
                item.DisplayName(),
                item.ProductSku,
                item.Quantity,
                item.UnitPrice,
                item.TotalPrice))
            .ToList(),
        order.TotalAmount,
        OrderStatusRules.GetNextStatuses(order.Status));
}
