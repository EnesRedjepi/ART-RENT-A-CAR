using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TextileShop.Services.Abstractions;
using TextileShop.Services.Models;
using TextileShop.Web.Extensions;
using TextileShop.Web.ViewModels;
using TextileShop.Web.Localization;

namespace TextileShop.Web.Areas.Admin.Controllers;

public class ProductsController : AdminControllerBase
{
    private const int PageSize = 15;

    private readonly IProductManagementService productService;
    private readonly ICategoryService categoryService;

    public ProductsController(IProductManagementService productService, ICategoryService categoryService)
    {
        this.productService = productService;
        this.categoryService = categoryService;
    }

    public async Task<IActionResult> Index([FromQuery] ProductFilterViewModel filter)
    {
        ViewBag.Categories = await LoadCategoryItemsAsync();
        ViewBag.Filter = filter;
        return View(await productService.SearchAsync(filter.ToQuery(PageSize)));
    }

    public async Task<IActionResult> Stock([FromQuery] int page = 1)
    {
        var query = new ProductQuery { Sort = ProductSort.StockAscending, Page = page, PageSize = PageSize };
        return View(await productService.SearchAsync(query));
    }

    public async Task<IActionResult> Create()
    {
        return View(await WithCategoriesAsync(new ProductFormViewModel()));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductFormViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await productService.CreateAsync(model.ToInput());
            if (result.Succeeded)
            {
                TempData["Success"] = this.Tr("Product created. You can now upload images.");
                return RedirectToAction(nameof(Edit), new { id = result.Value });
            }

            this.AddErrors(result);
        }

        return View(await WithCategoriesAsync(model));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var product = await productService.GetDetailsAsync(id);
        return product is null ? NotFound() : View(await WithCategoriesAsync(ProductFormViewModel.From(product)));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ProductFormViewModel model)
    {
        var existing = await productService.GetDetailsAsync(id);
        if (existing is null)
        {
            return NotFound();
        }

        model.Id = id;
        if (ModelState.IsValid)
        {
            var result = await productService.UpdateAsync(id, model.ToInput());
            if (result.Succeeded)
            {
                TempData["Success"] = this.Tr("Product saved.");
                return RedirectToAction(nameof(Edit), new { id });
            }

            this.AddErrors(result);
        }

        model.Images = existing.Images;
        return View(await WithCategoriesAsync(model));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SetActive(int id, bool isActive, string? returnUrl = null)
    {
        var result = await productService.SetActiveAsync(id, isActive);
        SetResultMessage(result, isActive ? "Product activated." : "Product deactivated.");
        return Url.IsLocalUrl(returnUrl) ? LocalRedirect(returnUrl) : RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStock(int id, int stockQuantity, int page = 1)
    {
        var result = await productService.UpdateStockAsync(id, stockQuantity);
        SetResultMessage(result, "Stock updated.");
        return RedirectToAction(nameof(Stock), new { page });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await productService.DeleteAsync(id);
        SetResultMessage(result, "Product deleted.");
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UploadImages(int id, IFormFileCollection files)
    {
        var result = await productService.AddImagesAsync(id, files.ToImageUploads());
        SetResultMessage(result, "Images uploaded.");
        return RedirectToAction(nameof(Edit), new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteImage(int id, int imageId)
    {
        var result = await productService.RemoveImageAsync(id, imageId);
        SetResultMessage(result, "Image removed.");
        return RedirectToAction(nameof(Edit), new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SetPrimaryImage(int id, int imageId)
    {
        var result = await productService.SetPrimaryImageAsync(id, imageId);
        SetResultMessage(result, "Primary image updated.");
        return RedirectToAction(nameof(Edit), new { id });
    }

    private async Task<ProductFormViewModel> WithCategoriesAsync(ProductFormViewModel model)
    {
        model.Categories = await LoadCategoryItemsAsync();
        return model;
    }

    private async Task<IReadOnlyList<SelectListItem>> LoadCategoryItemsAsync()
    {
        var categories = await categoryService.GetAllAsync();
        return categories.Select(category => new SelectListItem(category.Name, category.Id.ToString())).ToList();
    }
}
