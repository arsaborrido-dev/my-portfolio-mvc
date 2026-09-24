using my_portfolio_mvc.ViewModels.Contact;

namespace my_portfolio_mvc.Api.Interfaces;
public interface IContactApiClient
{
    public Task<(bool Success, string? Error)> SendContactMessageAsync(ContactMessageViewModel vm);
}
