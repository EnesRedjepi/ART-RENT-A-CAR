using Microsoft.Extensions.Logging;
using TextileShop.Domain.Entities;
using TextileShop.Domain.Enums;
using TextileShop.Services.Abstractions;
using TextileShop.Services.Common;
using TextileShop.Services.Models;

namespace TextileShop.Services.Implementations;

public class OrderService : IOrderService
{
    private const int CustomerOrdersPageSize = 10;
    private const int MaxPageSize = 100;

    private readonly IOrderRepository orderRepository;
    private readonly ICartRepository cartRepository;
    private readonly IAddressRepository addressRepository;
    private readonly IIdentityService identityService;
    private readonly IPricingService pricingService;
    private readonly IUnitOfWork unitOfWork;
    private readonly TimeProvider timeProvider;
    private readonly ILogger<OrderService> logger;

    public OrderService(
        IOrderRepository orderRepository,
        ICartRepository cartRepository,
        IAddressRepository addressRepository,
        IIdentityService identityService,
        IPricingService pricingService,
        IUnitOfWork unitOfWork,
        TimeProvider timeProvider,
        ILogger<OrderService> logger)
    {
        this.orderRepository = orderRepository;
        this.cartRepository = cartRepository;
        this.addressRepository = addressRepository;
        this.identityService = identityService;
        this.pricingService = pricingService;
        this.unitOfWork = unitOfWork;
        this.timeProvider = timeProvider;
        this.logger = logger;
    }

    public async Task<ServiceResult<int>> PlaceOrderAsync(string userId, CheckoutInput input)
    {
        var addressErrors = AddressRules.Validate(input.Address);
        if (addressErrors.Count > 0)
        {
            return ServiceResult<int>.Failure(addressErrors);
        }

        var cartItems = await cartRepository.GetItemsAsync(userId);
        if (cartItems.Count == 0)
        {
            return ServiceResult<int>.Failure("Your cart is empty.");
        }

        var stockErrors = FindStockErrors(cartItems);
        if (stockErrors.Count > 0)
        {
            return ServiceResult<int>.Failure(stockErrors);
        }

        var accountType = await identityService.GetAccountTypeAsync(userId);
        var order = BuildOrder(userId, input, cartItems, accountType);

        await SaveDefaultAddressIfMissingAsync(userId, order.ShippingAddress!);
        await orderRepository.AddAsync(order);
        cartRepository.RemoveRange(cartItems);

        try
        {
            await unitOfWork.SaveChangesAsync();
        }
        catch (ConcurrencyConflictException exception)
        {
            logger.LogError(exception, "Order placement failed for user {UserId} because stock changed", userId);
            return ServiceResult<int>.Failure("Stock levels changed while you were checking out. Please review your cart and try again.");
        }

        logger.LogInformation(
            "Order {OrderNumber} created for user {UserId} with total {TotalAmount}",
            order.OrderNumber,
            userId,
            order.TotalAmount);

        return ServiceResult<int>.Success(order.Id);
    }

    public async Task<PagedResult<OrderListItem>> GetCustomerOrdersAsync(string userId, int page)
    {
        var orders = await orderRepository.GetForCustomerAsync(userId, Math.Max(1, page), CustomerOrdersPageSize);
        return orders.Map(order => order.ToListItem());
    }

    public async Task<OrderDetails?> GetCustomerOrderAsync(string userId, int orderId)
    {
        var order = await orderRepository.GetByIdAsync(orderId);
        return order is not null && order.CustomerId == userId ? order.ToDetails() : null;
    }

    public async Task<ServiceResult> CancelCustomerOrderAsync(string userId, int orderId)
    {
        var order = await orderRepository.GetByIdAsync(orderId);
        if (order is null || order.CustomerId != userId)
        {
            return ServiceResult.Failure("Order was not found.");
        }

        if (order.Status != OrderStatus.Pending)
        {
            return ServiceResult.Failure("Only pending orders can be cancelled. Please contact us for further help.");
        }

        return await ChangeStatusAsync(order, OrderStatus.Cancelled);
    }

    public async Task<PagedResult<OrderListItem>> SearchAsync(OrderQuery query)
    {
        query.Page = Math.Max(1, query.Page);
        query.PageSize = Math.Clamp(query.PageSize, 1, MaxPageSize);

        var orders = await orderRepository.SearchAsync(query);
        return orders.Map(order => order.ToListItem());
    }

    public async Task<OrderDetails?> GetAsync(int orderId)
    {
        var order = await orderRepository.GetByIdAsync(orderId);
        return order?.ToDetails();
    }

    public async Task<ServiceResult> UpdateStatusAsync(int orderId, OrderStatus status)
    {
        var order = await orderRepository.GetByIdAsync(orderId);
        if (order is null)
        {
            return ServiceResult.Failure("Order was not found.");
        }

        return await ChangeStatusAsync(order, status);
    }

    public async Task<ServiceResult> UpdatePaymentStatusAsync(int orderId, PaymentStatus status)
    {
        var order = await orderRepository.GetByIdAsync(orderId);
        if (order is null)
        {
            return ServiceResult.Failure("Order was not found.");
        }

        order.PaymentStatus = status;
        await unitOfWork.SaveChangesAsync();

        logger.LogInformation("Order {OrderNumber} payment status set to {PaymentStatus}", order.OrderNumber, status);
        return ServiceResult.Success();
    }

    private async Task<ServiceResult> ChangeStatusAsync(Order order, OrderStatus newStatus)
    {
        if (!OrderStatusRules.CanMove(order.Status, newStatus))
        {
            return ServiceResult.Failure($"An order that is {order.Status} cannot be changed to {newStatus}.");
        }

        var previousStatus = order.Status;
        order.Status = newStatus;
        ApplyStatusSideEffects(order, newStatus);

        try
        {
            await unitOfWork.SaveChangesAsync();
        }
        catch (ConcurrencyConflictException exception)
        {
            logger.LogError(exception, "Status change failed for order {OrderNumber}", order.OrderNumber);
            return ServiceResult.Failure("The order was changed by someone else. Please reload and try again.");
        }

        logger.LogInformation(
            "Order {OrderNumber} status changed from {PreviousStatus} to {NewStatus}",
            order.OrderNumber,
            previousStatus,
            newStatus);

        return ServiceResult.Success();
    }

    private static void ApplyStatusSideEffects(Order order, OrderStatus newStatus)
    {
        if (newStatus == OrderStatus.Delivered && order.PaymentStatus == PaymentStatus.Pending)
        {
            order.PaymentStatus = PaymentStatus.Paid;
        }

        if (newStatus != OrderStatus.Cancelled)
        {
            return;
        }

        foreach (var item in order.Items)
        {
            item.Product!.IncreaseStock(item.Quantity);
        }

        if (order.PaymentStatus == PaymentStatus.Paid)
        {
            order.PaymentStatus = PaymentStatus.Refunded;
        }
    }

    private static List<string> FindStockErrors(IReadOnlyList<CartItem> cartItems)
    {
        var errors = new List<string>();

        foreach (var item in cartItems)
        {
            var product = item.Product!;
            if (!product.IsActive)
            {
                errors.Add($"{product.DisplayName()} is no longer available. Please remove it from your cart.");
            }
            else if (!product.HasStockFor(item.Quantity))
            {
                errors.Add($"Only {product.StockQuantity} units of {product.DisplayName()} are in stock.");
            }
        }

        return errors;
    }

    private Order BuildOrder(string userId, CheckoutInput input, IReadOnlyList<CartItem> cartItems, AccountType accountType)
    {
        var now = timeProvider.GetUtcNow().UtcDateTime;
        var order = new Order
        {
            OrderNumber = OrderNumberGenerator.Next(now),
            CustomerId = userId,
            OrderDate = now,
            Status = OrderStatus.Pending,
            PaymentStatus = PaymentStatus.Pending,
            ShippingAddress = input.Address.ToEntity(),
            Notes = input.Notes?.Trim()
        };

        foreach (var cartItem in cartItems)
        {
            var product = cartItem.Product!;
            var price = pricingService.GetPrice(product, cartItem.Quantity, accountType);

            product.DecreaseStock(cartItem.Quantity);
            product.UpdatedAt = now;
            order.Items.Add(new OrderItem
            {
                Product = product,
                ProductName = product.Name,
                ProductSku = product.Sku,
                Quantity = cartItem.Quantity,
                UnitPrice = price.UnitPrice
            });
        }

        order.RecalculateTotal();
        return order;
    }

    private async Task SaveDefaultAddressIfMissingAsync(string userId, Address shippingAddress)
    {
        if (await addressRepository.GetDefaultForUserAsync(userId) is not null)
        {
            return;
        }

        var defaultAddress = shippingAddress.CopyForOrder();
        defaultAddress.UserId = userId;
        await addressRepository.AddAsync(defaultAddress);
    }
}
