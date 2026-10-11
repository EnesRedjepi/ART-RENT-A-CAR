namespace TextileShop.Services.Abstractions;

public interface IImageStorage
{
    Task<string> SaveAsync(Stream content, string extension);

    Task DeleteAsync(string fileName);
}
