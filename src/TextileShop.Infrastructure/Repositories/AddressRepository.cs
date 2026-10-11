using Microsoft.EntityFrameworkCore;
using TextileShop.Domain.Entities;
using TextileShop.Infrastructure.Data;
using TextileShop.Services.Abstractions;

namespace TextileShop.Infrastructure.Repositories;

public class AddressRepository : IAddressRepository
{
    private readonly AppDbContext context;

    public AddressRepository(AppDbContext context)
    {
        this.context = context;
    }

    public Task<Address?> GetDefaultForUserAsync(string userId)
    {
        return context.Addresses
            .Where(address => address.UserId == userId)
            .OrderBy(address => address.Id)
            .FirstOrDefaultAsync();
    }

    public async Task AddAsync(Address address)
    {
        await context.Addresses.AddAsync(address);
    }
}
