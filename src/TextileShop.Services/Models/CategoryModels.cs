namespace TextileShop.Services.Models;

/// <summary>
/// <see cref="Name"/> and <see cref="Description"/> are in the current UI language (English as fallback);
/// <see cref="BaseName"/>, <see cref="BaseDescription"/> and <see cref="Translations"/> are the stored values, used by the admin forms.
/// </summary>
public record CategoryModel(
    int Id,
    string Name,
    string Slug,
    string? Description,
    int ProductCount,
    string BaseName,
    string? BaseDescription,
    IReadOnlyList<TextTranslation> Translations);

public class CategoryInput
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public IReadOnlyList<TextTranslation> Translations { get; set; } = Array.Empty<TextTranslation>();
}
