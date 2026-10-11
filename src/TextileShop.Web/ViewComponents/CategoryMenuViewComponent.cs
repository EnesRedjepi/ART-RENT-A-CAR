using Microsoft.AspNetCore.Mvc;
using TextileShop.Services.Abstractions;

namespace TextileShop.Web.ViewComponents;

public class CategoryMenuViewComponent : ViewComponent
{
    private readonly ICategoryService categoryService;

    public CategoryMenuViewComponent(ICategoryService categoryService)
    {
        this.categoryService = categoryService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View(await categoryService.GetAllAsync());
    }
}
