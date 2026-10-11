using TextileShop.Domain.Entities;
using TextileShop.Services.Models;

namespace TextileShop.Services.Common;

/// <summary>The languages product and category text can be translated into, besides the English text stored on the entity.</summary>
public static class CatalogLanguages
{
    public static readonly IReadOnlyList<string> Codes = new[] { "sq", "mk", "tr" };

    public static bool IsTranslatable(string? culture) =>
        culture is not null && Codes.Contains(culture, StringComparer.OrdinalIgnoreCase);
}

internal static class TranslationSync
{
    /// <summary>Adds, updates or removes translations so they match <paramref name="input"/>; a language with nothing filled in is removed.</summary>
    public static void Apply(ICollection<ProductTranslation> translations, IEnumerable<TextTranslation> input)
    {
        foreach (var item in Normalize(input))
        {
            var existing = translations.FirstOrDefault(translation => translation.Culture == item.Culture);

            // A language with nothing filled in is removed; a blank name alone just means "show the English name".
            var isEmpty = item.Name.Length == 0 && item.Description is null && item.Brand is null && item.Material is null && item.Color is null && item.Size is null;
            if (isEmpty)
            {
                if (existing is not null)
                {
                    translations.Remove(existing);
                }

                continue;
            }

            if (existing is null)
            {
                existing = new ProductTranslation { Culture = item.Culture };
                translations.Add(existing);
            }

            existing.Name = item.Name;
            existing.Description = item.Description;
            existing.Brand = item.Brand;
            existing.Material = item.Material;
            existing.Color = item.Color;
            existing.Size = item.Size;
        }
    }

    public static void Apply(ICollection<CategoryTranslation> translations, IEnumerable<TextTranslation> input)
    {
        foreach (var item in Normalize(input))
        {
            var existing = translations.FirstOrDefault(translation => translation.Culture == item.Culture);

            if (item.Name.Length == 0)
            {
                if (existing is not null)
                {
                    translations.Remove(existing);
                }

                continue;
            }

            if (existing is null)
            {
                translations.Add(new CategoryTranslation { Culture = item.Culture, Name = item.Name, Description = item.Description });
                continue;
            }

            existing.Name = item.Name;
            existing.Description = item.Description;
        }
    }

    private static IEnumerable<TextTranslation> Normalize(IEnumerable<TextTranslation> input) => input
        .Where(item => CatalogLanguages.IsTranslatable(item.Culture))
        .GroupBy(item => item.Culture.ToLowerInvariant())
        .Select(group => group.Last())
        .Select(item => new TextTranslation(
            item.Culture.ToLowerInvariant(),
            item.Name?.Trim() ?? string.Empty,
            Clean(item.Description),
            Clean(item.Brand),
            Clean(item.Material),
            Clean(item.Color),
            Clean(item.Size)));

    private static string? Clean(string? value) => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
