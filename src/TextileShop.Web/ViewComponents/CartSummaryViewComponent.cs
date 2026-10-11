using Microsoft.AspNetCore.Mvc;
using TextileShop.Domain.Constants;
using TextileShop.Services.Abstractions;
using TextileShop.Web.Extensions;

namespace TextileShop.Web.ViewComponents;

public class CartSummaryViewComponent : ViewComponent
{
    private readonly ICartService cartService;

    public CartSummaryViewComponent(ICartService cartService)
    {
        this.cartService = cartService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var isCustomer = UserClaimsPrincipal.IsInRole(Roles.RetailCustomer) || UserClaimsPrincipal.IsInRole(Roles.WholesaleCustomer);
        var itemCount = isCustomer ? await cartService.GetItemCountAsync(UserClaimsPrincipal.GetUserId()) : 0;
        return View(itemCount);
    }
}
