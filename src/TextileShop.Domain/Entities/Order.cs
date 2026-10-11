using TextileShop.Domain.Enums;

namespace TextileShop.Domain.Entities;

public class Order : BaseEntity
{
    public string OrderNumber { get; set; } = string.Empty;

    public string CustomerId { get; set; } = string.Empty;

    public ApplicationUser? Customer { get; set; }

    public DateTime OrderDate { get; set; }

    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

    public int ShippingAddressId { get; set; }

    public Address? ShippingAddress { get; set; }

    public string? Notes { get; set; }

    public decimal TotalAmount { get; set; }

    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

    public void RecalculateTotal()
    {
        TotalAmount = Items.Sum(item => item.TotalPrice);
    }
}
