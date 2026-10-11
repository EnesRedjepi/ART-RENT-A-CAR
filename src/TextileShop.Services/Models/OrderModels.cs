using TextileShop.Domain.Enums;

namespace TextileShop.Services.Models;

public record AddressData(
    string RecipientName,
    string PhoneNumber,
    string Street,
    string City,
    string PostalCode,
    string Country);

public class CheckoutInput
{
    public AddressData Address { get; set; } = new(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);

    public string? Notes { get; set; }
}

public class OrderQuery
{
    public string? Search { get; set; }

    public OrderStatus? Status { get; set; }

    public PaymentStatus? PaymentStatus { get; set; }

    public DateTime? From { get; set; }

    public DateTime? To { get; set; }

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 15;
}

public record OrderListItem(
    int Id,
    string OrderNumber,
    DateTime OrderDate,
    OrderStatus Status,
    PaymentStatus PaymentStatus,
    string CustomerName,
    string CustomerEmail,
    int ItemCount,
    decimal TotalAmount);

public record OrderItemModel(
    int ProductId,
    string ProductName,
    string Sku,
    int Quantity,
    decimal UnitPrice,
    decimal TotalPrice);

public record OrderDetails(
    int Id,
    string OrderNumber,
    DateTime OrderDate,
    OrderStatus Status,
    PaymentStatus PaymentStatus,
    string CustomerName,
    string CustomerEmail,
    AddressData ShippingAddress,
    string? Notes,
    IReadOnlyList<OrderItemModel> Items,
    decimal TotalAmount,
    IReadOnlyList<OrderStatus> AllowedNextStatuses)
{
    public bool CanBeCancelledByCustomer => Status == OrderStatus.Pending;
}
