using Microsoft.AspNetCore.Mvc;
using TextileShop.Services.Abstractions;

namespace TextileShop.Web.Areas.Admin.Controllers;

public class DashboardController : AdminControllerBase
{
    private readonly IDashboardService dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        this.dashboardService = dashboardService;
    }

    public async Task<IActionResult> Index()
    {
        return View(await dashboardService.GetStatsAsync());
    }
}
