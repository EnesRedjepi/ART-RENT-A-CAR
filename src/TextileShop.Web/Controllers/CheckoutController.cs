using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TextileShop.Domain.Constants;
using TextileShop.Services.Abstractions;
using TextileShop.Web.Extensions;
using TextileShop.Web.ViewModels;
using TextileShop.Web.Localization;

namespace TextileShop.Web.Controllers;

[Authorize(Roles = Roles.Customers)]
public class CheckoutController : Controller
{
    private readonly ICartService cartService;
    private readonly IOrderService orderService;
    private readonly IAddressService addressService;
    private readonly IIdentityService identityService;

    public CheckoutController(
        ICartService cartService,
        IOrderService orderService,
        IAddressService addressService,
        IIdentityService identityService)
    {
        this.cartService = cartService;
        this.orderService = orderService;
        this.addressService = addressService;
        this.identityService = identityService;
    }

    public async Task<IActionResult> Index()
    {
        var cart = await cartService.GetCartAsync(User.GetUserId());
        if (cart.IsEmpty)
        {
            return RedirectToAction("Index", "Cart");
        }

        return View(new CheckoutViewModel { Address = await LoadSuggestedAddressAsync(), Cart = cart });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(CheckoutViewModel model)
    {
        var userId = User.GetUserId();

        if (ModelState.IsValid)
        {
            var result = await orderService.PlaceOrderAsync(userId, model.ToInput());
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Confirmation), new { id = result.Value });
            }

            this.AddErrors(result);
        }

        model.Cart = await cartService.GetCartAsync(userId);
        return View(model);
    }

    public async Task<IActionResult> Confirmation(int id)
    {
        var order = await orderService.GetCustomerOrderAsync(User.GetUserId(), id);
        return order is null ? NotFound() : View(order);
    }

    private async Task<AddressViewModel> LoadSuggestedAddressAsync()
    {
        var userId = User.GetUserId();
        var savedAddress = await addressService.GetDeliveryAddressAsync(userId);
        if (savedAddress is not null)
        {
            return AddressViewModel.From(savedAddress);
        }

        var customer = await identityService.GetCustomerAsync(userId);
        return new AddressViewModel { RecipientName = customer?.FullName ?? string.Empty, PhoneNumber = customer?.PhoneNumber ?? string.Empty };
    }
}
