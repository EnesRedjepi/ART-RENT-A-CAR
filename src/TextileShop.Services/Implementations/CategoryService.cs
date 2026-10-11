using Microsoft.Extensions.Logging;
using TextileShop.Domain.Entities;
using TextileShop.Services.Abstractions;
using TextileShop.Services.Common;
using TextileShop.Services.Models;

namespace TextileShop.Services.Implementations;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository categoryRepository;
    private readonly IUnitOfWork unitOfWork;
    private readonly ILogger<CategoryService> logger;

    public CategoryService(
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork,
        ILogger<CategoryService> logger)
    {
        this.categoryRepository = categoryRepository;
        this.unitOfWork = unitOfWork;
        this.logger = logger;
    }

    public async Task<IReadOnlyList<CategoryModel>> GetAllAsync()
    {
        var categories = await categoryRepository.GetAllWithProductCountsAsync();
        return categories.Select(entry => ToModel(entry.Category, entry.ProductCount)).ToList();
    }

    public async Task<CategoryModel?> GetByIdAsync(int id)
    {
        var category = await categoryRepository.GetByIdAsync(id);
        return category is null ? null : ToModel(category, 0);
    }

    public async Task<CategoryModel?> GetBySlugAsync(string slug)
    {
        var category = await categoryRepository.GetBySlugAsync(slug);
        return category is null ? null : ToModel(category, 0);
    }

    public async Task<ServiceResult> CreateAsync(CategoryInput input)
    {
        var errors = await ValidateAsync(input, null);
        if (errors.Count > 0)
        {
            return ServiceResult.Failure(errors);
        }

        var category = new Category();
        Apply(category, input);

        await categoryRepository.AddAsync(category);
        await unitOfWork.SaveChangesAsync();

        logger.LogInformation("Category {CategoryId} created", category.Id);
        return ServiceResult.Success();
    }

    public async Task<ServiceResult> UpdateAsync(int id, CategoryInput input)
    {
        var category = await categoryRepository.GetByIdAsync(id);
        if (category is null)
        {
            return ServiceResult.Failure("Category was not found.");
        }

        var errors = await ValidateAsync(input, id);
        if (errors.Count > 0)
        {
            return ServiceResult.Failure(errors);
        }

        Apply(category, input);
        await unitOfWork.SaveChangesAsync();

        logger.LogInformation("Category {CategoryId} updated", id);
        return ServiceResult.Success();
    }

    public async Task<ServiceResult> DeleteAsync(int id)
    {
        var category = await categoryRepository.GetByIdAsync(id);
        if (category is null)
        {
            return ServiceResult.Failure("Category was not found.");
        }

        if (await categoryRepository.HasProductsAsync(id))
        {
            return ServiceResult.Failure("This category still contains products and cannot be deleted.");
        }

        categoryRepository.Remove(category);
        await unitOfWork.SaveChangesAsync();

        logger.LogInformation("Category {CategoryId} deleted", id);
        return ServiceResult.Success();
    }

    private static CategoryModel ToModel(Category category, int productCount) => new(
        category.Id,
        category.DisplayName(),
        category.Slug,
        category.DisplayDescription(),
        productCount,
        category.Name,
        category.Description,
        category.Translations.ToModels());

    private static void Apply(Category category, CategoryInput input)
    {
        category.Name = input.Name.Trim();
        category.Slug = SlugGenerator.Generate(input.Name);
        category.Description = input.Description?.Trim();
        TranslationSync.Apply(category.Translations, input.Translations);
    }

    private async Task<IReadOnlyList<string>> ValidateAsync(CategoryInput input, int? categoryId)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(input.Name))
        {
            errors.Add("Category name is required.");
            return errors;
        }

        var slugOwner = await categoryRepository.GetBySlugAsync(SlugGenerator.Generate(input.Name));
        var nameTaken = await categoryRepository.NameExistsAsync(input.Name.Trim(), categoryId);
        if (nameTaken || (slugOwner is not null && slugOwner.Id != categoryId))
        {
            errors.Add("A category with this name already exists.");
        }

        return errors;
    }
}
