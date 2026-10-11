using Microsoft.AspNetCore.Mvc;
using TextileShop.Services.Abstractions;
using TextileShop.Web.Extensions;
using TextileShop.Web.ViewModels;
using TextileShop.Web.Localization;

namespace TextileShop.Web.Areas.Admin.Controllers;

public class CategoriesController : AdminControllerBase
{
    private readonly ICategoryService categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        this.categoryService = categoryService;
    }

    public async Task<IActionResult> Index()
    {
        return View(await categoryService.GetAllAsync());
    }

    public IActionResult Create()
    {
        return View(new CategoryFormViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CategoryFormViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await categoryService.CreateAsync(model.ToInput());
            if (result.Succeeded)
            {
                TempData["Success"] = this.Tr("Category created.");
                return RedirectToAction(nameof(Index));
            }

            this.AddErrors(result);
        }

        return View(model);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var category = await categoryService.GetByIdAsync(id);
        return category is null ? NotFound() : View(CategoryFormViewModel.From(category));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CategoryFormViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await categoryService.UpdateAsync(id, model.ToInput());
            if (result.Succeeded)
            {
                TempData["Success"] = this.Tr("Category saved.");
                return RedirectToAction(nameof(Index));
            }

            this.AddErrors(result);
        }

        model.Id = id;
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await categoryService.DeleteAsync(id);
        SetResultMessage(result, "Category deleted.");
        return RedirectToAction(nameof(Index));
    }
}
