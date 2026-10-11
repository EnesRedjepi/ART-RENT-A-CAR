using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TextileShop.Services.Abstractions;
using TextileShop.Services.Models;
using TextileShop.Web.ViewModels;

namespace TextileShop.Web.Controllers;

public class HomeController : Controller
{
    private const int NewArrivalCount = 8;

    private readonly ICatalogService catalogService;
    private readonly ICategoryService categoryService;

    public HomeController(ICatalogService catalogService, ICategoryService categoryService)
    {
        this.catalogService = catalogService;
        this.categoryService = categoryService;
    }

    public async Task<IActionResult> Index()
    {
        var newArrivals = await catalogService.SearchAsync(new ProductQuery { PageSize = NewArrivalCount });

        return View(new HomeViewModel
        {
            Categories = await categoryService.GetAllAsync(),
            NewArrivals = newArrivals.Items
        });
    }

    public IActionResult HttpStatus(int code)
    {
        Response.StatusCode = code;
        return View("Error", new ErrorViewModel { StatusCode = code, RequestId = HttpContext.TraceIdentifier });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { StatusCode = 500, RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
