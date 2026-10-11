using Microsoft.EntityFrameworkCore;
using TextileShop.Domain.Entities;
using TextileShop.Infrastructure.Data;
using TextileShop.Services.Abstractions;

namespace TextileShop.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext context;

    public CategoryRepository(AppDbContext context)
    {
        this.context = context;
    }

    public async Task<IReadOnlyList<(Category Category, int ProductCount)>> GetAllWithProductCountsAsync()
    {
        var categories = await context.Categories
            .AsNoTracking()
            .Include(category => category.Translations)
            .OrderBy(category => category.Name)
            .ToListAsync();

        var counts = await context.Products
            .AsNoTracking()
            .Where(product => product.IsActive)
            .GroupBy(product => product.CategoryId)
            .Select(group => new { CategoryId = group.Key, Count = group.Count() })
            .ToDictionaryAsync(row => row.CategoryId, row => row.Count);

        return categories.Select(category => (category, counts.GetValueOrDefault(category.Id))).ToList();
    }

    public Task<Category?> GetByIdAsync(int id)
    {
        return context.Categories.Include(category => category.Translations).FirstOrDefaultAsync(category => category.Id == id);
    }

    public Task<Category?> GetBySlugAsync(string slug)
    {
        return context.Categories.Include(category => category.Translations).FirstOrDefaultAsync(category => category.Slug == slug);
    }

    public Task<bool> NameExistsAsync(string name, int? excludedCategoryId)
    {
        return context.Categories.AnyAsync(category => category.Name == name && category.Id != excludedCategoryId);
    }

    public Task<bool> HasProductsAsync(int categoryId)
    {
        return context.Products.AnyAsync(product => product.CategoryId == categoryId);
    }

    public async Task AddAsync(Category category)
    {
        await context.Categories.AddAsync(category);
    }

    public void Remove(Category category)
    {
        context.Categories.Remove(category);
    }
}
