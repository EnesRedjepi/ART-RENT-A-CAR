namespace TextileShop.Domain.Entities;

public class ProductImage : BaseEntity
{
    public int ProductId { get; set; }

    public Product? Product { get; set; }

    public string FileName { get; set; } = string.Empty;

    public bool IsPrimary { get; set; }
}
