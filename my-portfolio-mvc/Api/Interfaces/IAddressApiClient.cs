
using my_portfolio_mvc.Api.Models.Address;
using my_portfolio_mvc.ViewModels.Address;

namespace my_portfolio_mvc.Api.Interfaces;
public interface IAddressApiClient
{
    Task<AddressDto?> GetAddressByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<List<AddressDto>> GetAddressesByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    Task<List<AddressDto>> GetAllAddressesAsync(CancellationToken cancellationToken = default);
    Task<AddressDto> AddAddressAsync(AddressViewModel vm, CancellationToken cancellationToken = default);
    Task<bool> UpdateAddressAsync(AddressViewModel vm, CancellationToken cancellationToken = default);
    Task<bool> DeleteAddressAsync(long id, CancellationToken cancellationToken = default);


}
