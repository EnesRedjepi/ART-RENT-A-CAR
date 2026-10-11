using Microsoft.AspNetCore.Mvc;
using TextileShop.Domain.Enums;
using TextileShop.Services.Abstractions;
using TextileShop.Web.ViewModels;
using TextileShop.Web.Localization;

namespace TextileShop.Web.Areas.Admin.Controllers;

public class OrdersController : AdminControllerBase
{
    private readonly IOrderService orderService;

    public OrdersController(IOrderService orderService)
    {
        this.orderService = orderService;
    }

    public async Task<IActionResult> Index([FromQuery] OrderFilterViewModel filter)
    {
        ViewBag.Filter = filter;
        return View(await orderService.SearchAsync(filter.ToQuery()));
    }

    public async Task<IActionResult> Details(int id)
    {
        var order = await orderService.GetAsync(id);
        return order is null ? NotFound() : View(order);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatus(int id, OrderStatus status)
    {
        var result = await orderService.UpdateStatusAsync(id, status);
        SetResultMessage(result, "Order status changed to {0}.", this.Tr(status.ToString()));
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdatePayment(int id, PaymentStatus paymentStatus)
    {
        var result = await orderService.UpdatePaymentStatusAsync(id, paymentStatus);
        SetResultMessage(result, "Payment status changed to {0}.", this.Tr(paymentStatus.ToString()));
        return RedirectToAction(nameof(Details), new { id });
    }
}
