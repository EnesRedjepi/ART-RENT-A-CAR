using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TextileShop.Domain.Constants;
using TextileShop.Services.Abstractions;
using TextileShop.Web.Extensions;
using TextileShop.Web.Localization;

namespace TextileShop.Web.Controllers;

[Authorize(Roles = Roles.Customers)]
public class OrdersController : Controller
{
    private readonly IOrderService orderService;

    public OrdersController(IOrderService orderService)
    {
        this.orderService = orderService;
    }

    public async Task<IActionResult> Index(int page = 1)
    {
        return View(await orderService.GetCustomerOrdersAsync(User.GetUserId(), page));
    }

    public async Task<IActionResult> Details(int id)
    {
        var order = await orderService.GetCustomerOrderAsync(User.GetUserId(), id);
        return order is null ? NotFound() : View(order);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel(int id)
    {
        var result = await orderService.CancelCustomerOrderAsync(User.GetUserId(), id);
        TempData[result.Succeeded ? "Success" : "Error"] = result.Succeeded ? this.Tr("Your order was cancelled.") : this.ErrorText(result);

        return RedirectToAction(nameof(Details), new { id });
    }
}
