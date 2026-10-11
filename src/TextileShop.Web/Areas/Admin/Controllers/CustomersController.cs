using Microsoft.AspNetCore.Mvc;
using TextileShop.Domain.Enums;
using TextileShop.Services.Abstractions;
using TextileShop.Web.ViewModels;
using TextileShop.Web.Localization;

namespace TextileShop.Web.Areas.Admin.Controllers;

public class CustomersController : AdminControllerBase
{
    private readonly IIdentityService identityService;

    public CustomersController(IIdentityService identityService)
    {
        this.identityService = identityService;
    }

    public async Task<IActionResult> Index([FromQuery] CustomerFilterViewModel filter)
    {
        ViewBag.Filter = filter;
        return View(await identityService.SearchCustomersAsync(filter.ToQuery()));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangeAccountType(string id, AccountType accountType, string? returnUrl = null)
    {
        var result = await identityService.ChangeAccountTypeAsync(id, accountType);
        SetResultMessage(result, "Account type changed to {0}.", this.Tr(accountType.ToString()));
        return Url.IsLocalUrl(returnUrl) ? LocalRedirect(returnUrl) : RedirectToAction(nameof(Index));
    }
}
