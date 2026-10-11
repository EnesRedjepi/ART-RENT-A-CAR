using Microsoft.Extensions.Logging;
using TextileShop.Services.Abstractions;
using TextileShop.Services.Common;
using TextileShop.Services.Models;

namespace TextileShop.Services.Implementations;

public class AddressService : IAddressService
{
    private readonly IAddressRepository addressRepository;
    private readonly IUnitOfWork unitOfWork;
    private readonly ILogger<AddressService> logger;

    public AddressService(
        IAddressRepository addressRepository,
        IUnitOfWork unitOfWork,
        ILogger<AddressService> logger)
    {
        this.addressRepository = addressRepository;
        this.unitOfWork = unitOfWork;
        this.logger = logger;
    }

    public async Task<AddressData?> GetDeliveryAddressAsync(string userId)
    {
        var address = await addressRepository.GetDefaultForUserAsync(userId);
        return address?.ToData();
    }

    public async Task<ServiceResult> SaveDeliveryAddressAsync(string userId, AddressData data)
    {
        var errors = AddressRules.Validate(data);
        if (errors.Count > 0)
        {
            return ServiceResult.Failure(errors);
        }

        var newAddress = data.ToEntity();
        var existingAddress = await addressRepository.GetDefaultForUserAsync(userId);
        if (existingAddress is null)
        {
            newAddress.UserId = userId;
            await addressRepository.AddAsync(newAddress);
        }
        else
        {
            existingAddress.CopyFrom(newAddress);
        }

        await unitOfWork.SaveChangesAsync();
        logger.LogInformation("Delivery address saved for user {UserId}", userId);
        return ServiceResult.Success();
    }
}
