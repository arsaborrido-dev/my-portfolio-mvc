using my_portfolio_mvc.Api.Models.Phone;
using my_portfolio_mvc.ViewModels.Phone;

namespace my_portfolio_mvc.Api.Interfaces;

public interface IPhoneApiClient
{
    Task<PhoneDto?> GetPhoneByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<List<PhoneDto>> GetAllPhonesAsync(CancellationToken cancellationToken = default);
    Task<PhoneDto> AddPhoneAsync(PhoneViewModel vm, CancellationToken cancellationToken = default);
    Task<bool> UpdatePhoneAsync(PhoneViewModel vm, CancellationToken cancellationToken = default);
    Task<bool> DeletePhoneAsync(long id, CancellationToken cancellationToken = default);
}
