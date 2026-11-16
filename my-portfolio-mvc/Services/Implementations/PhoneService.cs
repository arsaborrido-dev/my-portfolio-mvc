using my_portfolio_mvc.Api.Interfaces;
using my_portfolio_mvc.Api.Models.Phone;
using my_portfolio_mvc.Services.Interfaces;
using my_portfolio_mvc.ViewModels.Phone;

namespace my_portfolio_mvc.Services.Implementations;

public sealed class PhoneService : IPhoneService
{
    private readonly IPhoneApiClient _phoneApi;

    public PhoneService(IPhoneApiClient phoneApi) => _phoneApi = phoneApi;
    public async Task<PhoneDto> AddPhoneAsync(PhoneViewModel vm, CancellationToken cancellationToken = default)
    {
        return await _phoneApi.AddPhoneAsync(vm, cancellationToken);
    }

    public async Task<bool> DeletePhoneAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _phoneApi.DeletePhoneAsync(id, cancellationToken);
    }

    public async Task<List<PhoneViewModel>> GetAllPhonesAsync(CancellationToken cancellationToken = default)
    {
        var phones = await _phoneApi.GetAllPhonesAsync(cancellationToken);
        return phones?.Select(MapToVm).ToList() ?? new List<PhoneViewModel>();
    }
    private static PhoneViewModel MapToVm(PhoneDto dto)
    {
        return new PhoneViewModel
        {
            Id = dto.Id,
            AreaCode = dto.AreaCode,
            PhoneNumber = dto.PhoneNumber,
            CreatedAtUtc = dto.CreatedAtUtc,
            UpdatedAtUtc = dto.UpdatedAtUtc
        };
    }

    public async Task<bool> UpdatePhoneAsync(PhoneViewModel vm, CancellationToken cancellationToken = default)
    {
        return await _phoneApi.UpdatePhoneAsync(vm, cancellationToken);
    }
}
