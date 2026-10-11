using Microsoft.AspNetCore.Hosting;
using TextileShop.Services.Abstractions;

namespace TextileShop.Infrastructure.Storage;

public class LocalImageStorage : IImageStorage
{
    private readonly string directoryPath;

    public LocalImageStorage(IWebHostEnvironment environment)
    {
        directoryPath = Path.Combine(environment.WebRootPath ?? Path.Combine(environment.ContentRootPath, "wwwroot"), "uploads", "products");
    }

    public async Task<string> SaveAsync(Stream content, string extension)
    {
        Directory.CreateDirectory(directoryPath);

        var fileName = $"{Guid.NewGuid():N}{extension}";
        await using var target = File.Create(Path.Combine(directoryPath, fileName));
        await content.CopyToAsync(target);

        return fileName;
    }

    public Task DeleteAsync(string fileName)
    {
        var path = Path.Combine(directoryPath, Path.GetFileName(fileName));
        if (File.Exists(path))
        {
            File.Delete(path);
        }

        return Task.CompletedTask;
    }
}
