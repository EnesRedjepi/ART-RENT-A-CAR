namespace TextileShop.Services.Models;

public record CartLine(
    int ProductId,
    string ProductName,
    string Sku,
    string? ImageFileName,
    int Quantity,
    decimal UnitPrice,
    bool IsWholesalePrice,
    int StockQuantity,
    bool IsAvailable,
    int MinimumWholesaleQuantity)
{
    public decimal LineTotal => UnitPrice * Quantity;

    public bool HasStockIssue => !IsAvailable || Quantity > StockQuantity;
}

public record CartModel(IReadOnlyList<CartLine> Lines)
{
    public decimal Subtotal => Lines.Sum(line => line.LineTotal);

    public decimal Total => Subtotal;

    public int ItemCount => Lines.Sum(line => line.Quantity);

    public bool HasIssues => Lines.Any(line => line.HasStockIssue);

    public bool IsEmpty => Lines.Count == 0;
}
