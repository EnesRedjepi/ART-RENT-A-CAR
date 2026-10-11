namespace TextileShop.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string Sku { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public decimal WholesalePrice { get; set; }

    public int StockQuantity { get; set; }

    public int MinimumWholesaleQuantity { get; set; } = 1;

    public int CategoryId { get; set; }

    public Category? Category { get; set; }

    public string? Brand { get; set; }

    public string? Material { get; set; }

    public string? Size { get; set; }

    public string? Color { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();

    public ICollection<ProductTranslation> Translations { get; set; } = new List<ProductTranslation>();

    public bool HasStockFor(int quantity) => quantity > 0 && quantity <= StockQuantity;

    public void DecreaseStock(int quantity)
    {
        if (!HasStockFor(quantity))
        {
            throw new InvalidOperationException($"Insufficient stock for product {Sku}.");
        }

        StockQuantity -= quantity;
    }

    public void IncreaseStock(int quantity)
    {
        StockQuantity += quantity;
    }
}
