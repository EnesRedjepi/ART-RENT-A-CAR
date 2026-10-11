namespace TextileShop.Domain.Entities;

/// <summary>A product's name and description in one language. The English text lives on <see cref="Product"/> itself.</summary>
public class ProductTranslation : BaseEntity
{
    public int ProductId { get; set; }

    public Product? Product { get; set; }

    /// <summary>Two-letter ISO language code, e.g. "sq".</summary>
    public string Culture { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string? Brand { get; set; }

    public string? Material { get; set; }

    public string? Color { get; set; }

    public string? Size { get; set; }
}

/// <summary>A category's name and description in one language. The English text lives on <see cref="Category"/> itself.</summary>
public class CategoryTranslation : BaseEntity
{
    public int CategoryId { get; set; }

    public Category? Category { get; set; }

    /// <summary>Two-letter ISO language code, e.g. "sq".</summary>
    public string Culture { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }
}
