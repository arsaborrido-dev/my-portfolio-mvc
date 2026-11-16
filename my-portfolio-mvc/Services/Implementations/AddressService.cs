using my_portfolio_mvc.Api.Interfaces;
using my_portfolio_mvc.Api.Models.Address;
using my_portfolio_mvc.Api.Models.Project;
using my_portfolio_mvc.Services.Interfaces;
using my_portfolio_mvc.ViewModels.Address;
using System.Reflection.Metadata.Ecma335;

namespace my_portfolio_mvc.Services.Implementations;

public sealed class AddressService : IAddressService
{
    private readonly IAddressApiClient _addApi;
    public AddressService(IAddressApiClient addApi) => _addApi = addApi;
    public async Task<AddressViewModel?> GetAddressByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var address = await _addApi.GetAddressByIdAsync(id, cancellationToken);

        return (address is null
            ? null
            : MapToVm(address));
    }    
    public async Task<List<AddressViewModel>> GetAddressesByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        var uAddresses = await _addApi.GetAddressesByUserIdAsync(userId, cancellationToken);
        return uAddresses?.Select(MapToVm).ToList() ?? new List<AddressViewModel>();
    }

    public async Task<List<AddressViewModel>> GetAllAddressesAsync(CancellationToken cancellationToken = default)
    {
        var addresses = await _addApi.GetAllAddressesAsync(cancellationToken);      
        return addresses?.Select(MapToVm).ToList() ?? new List<AddressViewModel>();
    }
    private static AddressViewModel MapToVm(AddressDto dto)
    {
        return new AddressViewModel
        {
            Id = dto.Id,
            AddressLine1 = dto.AddressLine1,
            ZipCode = dto.ZipCode,
            UserId = dto.UserId,
            CreatedAtUtc = dto.CreatedAtUtc,
            UpdatedAtUtc = dto.UpdatedAtUtc
        };
    }

    public async Task<AddressDto> AddAddressAsync(AddressViewModel vm, CancellationToken cancellationToken = default)
    {
        return await _addApi.AddAddressAsync(vm, cancellationToken); //<--need to map to vm
    }

    public async Task<bool> UpdateAddressAsync(AddressViewModel vm, CancellationToken cancellationToken = default)
    {
        return await _addApi.UpdateAddressAsync(vm, cancellationToken);
    }

    public async Task<bool> DeleteAddressAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _addApi.DeleteAddressAsync(id, cancellationToken);
    }
}
