using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TextileShop.Domain.Constants;
using TextileShop.Services.Abstractions;
using TextileShop.Web.Extensions;
using TextileShop.Web.Localization;

namespace TextileShop.Web.Controllers;

[Authorize(Roles = Roles.Customers)]
public class CartController : Controller
{
    private readonly ICartService cartService;

    public CartController(ICartService cartService)
    {
        this.cartService = cartService;
    }

    public async Task<IActionResult> Index()
    {
        return View(await cartService.GetCartAsync(User.GetUserId()));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(int productId, int quantity = 1, string? returnUrl = null)
    {
        var result = await cartService.AddAsync(User.GetUserId(), productId, quantity);
        TempData[result.Succeeded ? "Success" : "Error"] = result.Succeeded ? this.Tr("Item added to your cart.") : this.ErrorText(result);

        return Url.IsLocalUrl(returnUrl) ? LocalRedirect(returnUrl) : RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int productId, int quantity)
    {
        var result = await cartService.SetQuantityAsync(User.GetUserId(), productId, quantity);
        if (!result.Succeeded)
        {
            TempData["Error"] = this.ErrorText(result);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Remove(int productId)
    {
        await cartService.RemoveAsync(User.GetUserId(), productId);
        return RedirectToAction(nameof(Index));
    }
}
