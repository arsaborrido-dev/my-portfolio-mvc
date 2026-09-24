using my_portfolio_mvc.Api.Interfaces;
using my_portfolio_mvc.Services.Interfaces;
using my_portfolio_mvc.ViewModels.Contact;

namespace my_portfolio_mvc.Services.Implementations;
public class ContactService : IContactService
{
    private readonly IContactApiClient _contactApi;
    public ContactService(IContactApiClient contactApi) => _contactApi = contactApi;
    public async Task<(bool Success, string? Error)> SendContactMessageAsync(ContactMessageViewModel vm)
    {
        return await _contactApi.SendContactMessageAsync(vm);            
    }
}
