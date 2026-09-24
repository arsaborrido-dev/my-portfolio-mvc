using my_portfolio_mvc.ViewModels.Contact;

namespace my_portfolio_mvc.Services.Interfaces;
public interface IContactService
{
    public Task<(bool Success, string? Error)> SendContactMessageAsync(ContactMessageViewModel vm);
}
