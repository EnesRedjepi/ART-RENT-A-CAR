using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TextileShop.Domain.Constants;
using TextileShop.Domain.Enums;
using TextileShop.Services.Abstractions;
using TextileShop.Web.Extensions;
using TextileShop.Web.ViewModels;
using TextileShop.Web.Localization;

namespace TextileShop.Web.Controllers;

public class AccountController : Controller
{
    private readonly IIdentityService identityService;
    private readonly IAddressService addressService;

    public AccountController(IIdentityService identityService, IAddressService addressService)
    {
        this.identityService = identityService;
        this.addressService = addressService;
    }

    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View(new LoginViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await identityService.LoginAsync(model.ToInput());
        if (!result.Succeeded)
        {
            this.AddErrors(result);
            return View(model);
        }

        if (Url.IsLocalUrl(returnUrl))
        {
            return LocalRedirect(returnUrl);
        }

        return result.Value == AccountType.Administrator
            ? RedirectToAction("Index", "Dashboard", new { area = "Admin" })
            : RedirectToAction("Index", "Home");
    }

    public IActionResult Register()
    {
        return View(new RegisterViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await identityService.RegisterAsync(model.ToInput());
        if (!result.Succeeded)
        {
            this.AddErrors(result);
            return View(model);
        }

        TempData["Success"] = this.Tr("Welcome! Your account has been created.");
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await identityService.LogoutAsync();
        return RedirectToAction("Index", "Home");
    }

    public IActionResult AccessDenied()
    {
        return View();
    }

    [Authorize]
    public async Task<IActionResult> Profile()
    {
        var customer = await identityService.GetCustomerAsync(User.GetUserId());
        return customer is null ? NotFound() : View(ProfileViewModel.From(customer));
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Profile(ProfileViewModel model)
    {
        var userId = User.GetUserId();
        var customer = await identityService.GetCustomerAsync(userId);
        if (customer is null)
        {
            return NotFound();
        }

        model.Email = customer.Email;
        model.AccountType = customer.AccountType;
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await identityService.UpdateProfileAsync(userId, model.ToInput());
        if (!result.Succeeded)
        {
            this.AddErrors(result);
            return View(model);
        }

        TempData["Success"] = this.Tr("Your profile was updated.");
        return RedirectToAction(nameof(Profile));
    }

    [Authorize(Roles = Roles.Customers)]
    public async Task<IActionResult> Address()
    {
        var address = await addressService.GetDeliveryAddressAsync(User.GetUserId());
        return View(address is null ? new AddressViewModel() : AddressViewModel.From(address));
    }

    [Authorize(Roles = Roles.Customers)]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Address(AddressViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await addressService.SaveDeliveryAddressAsync(User.GetUserId(), model.ToData());
        if (!result.Succeeded)
        {
            this.AddErrors(result);
            return View(model);
        }

        TempData["Success"] = this.Tr("Your delivery address was saved.");
        return RedirectToAction(nameof(Address));
    }
}
