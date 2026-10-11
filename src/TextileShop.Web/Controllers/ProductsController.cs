using Microsoft.AspNetCore.Mvc;
using TextileShop.Services.Abstractions;
using TextileShop.Services.Models;
using TextileShop.Web.ViewModels;

namespace TextileShop.Web.Controllers;

public class ProductsController : Controller
{
    private const int PageSize = 12;
    private const int RelatedProductCount = 4;

    private readonly ICatalogService catalogService;
    private readonly ICategoryService categoryService;

    public ProductsController(ICatalogService catalogService, ICategoryService categoryService)
    {
        this.catalogService = catalogService;
        this.categoryService = categoryService;
    }

    public async Task<IActionResult> Index([FromQuery] ProductFilterViewModel filter)
    {
        return View(await BuildCatalogAsync(filter, null));
    }

    [HttpGet("category/{slug}")]
    public async Task<IActionResult> Category(string slug, [FromQuery] ProductFilterViewModel filter)
    {
        var category = await categoryService.GetBySlugAsync(slug);
        if (category is null)
        {
            return NotFound();
        }

        filter.CategoryId = category.Id;
        return View("Index", await BuildCatalogAsync(filter, category));
    }

    public async Task<IActionResult> Details(int id)
    {
        var product = await catalogService.GetDetailsAsync(id);
        if (product is null)
        {
            return NotFound();
        }

        var related = await catalogService.SearchAsync(new ProductQuery
        {
            CategoryId = product.CategoryId,
            PageSize = RelatedProductCount + 1
        });

        return View(new ProductDetailsViewModel
        {
            Product = product,
            RelatedProducts = related.Items.Where(item => item.Id != id).Take(RelatedProductCount).ToList()
        });
    }

    private async Task<CatalogViewModel> BuildCatalogAsync(ProductFilterViewModel filter, CategoryModel? currentCategory)
    {
        return new CatalogViewModel
        {
            Products = await catalogService.SearchAsync(filter.ToQuery(PageSize)),
            Options = await catalogService.GetFilterOptionsAsync(),
            Categories = await categoryService.GetAllAsync(),
            Filter = filter,
            CurrentCategory = currentCategory
        };
    }
}
