using Microsoft.EntityFrameworkCore;
using TextileShop.Infrastructure.Data;
using TextileShop.Services.Abstractions;
using TextileShop.Services.Common;

namespace TextileShop.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext context;

    public UnitOfWork(AppDbContext context)
    {
        this.context = context;
    }

    public async Task SaveChangesAsync()
    {
        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException exception)
        {
            throw new ConcurrencyConflictException(exception);
        }
    }
}
