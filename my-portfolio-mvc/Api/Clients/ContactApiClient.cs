
//using static System.Net.WebRequestMethods;
using my_portfolio_mvc.ViewModels.Contact;
using my_portfolio_mvc.Api.Interfaces;
using my_portfolio_mvc.Api.Models;

namespace my_portfolio_mvc.Api.Clients;
public class ContactApiClient : IContactApiClient
{
    private readonly HttpClient _httpClient;
    public ContactApiClient(HttpClient httpClient) => _httpClient = httpClient;  
    public async Task<(bool Success, string? Error)> SendContactMessageAsync(ContactMessageViewModel vm)
    {
        try
        {
            var payload = new ContactMessageDto
            {
                Name = vm.Name,
                Email = vm.Email,
                Message = vm.Message,
                CaptchaToken = vm.CaptchaToken
            };

            var response = await _httpClient.PostAsJsonAsync(
                "api/contacts/message", 
                payload                    
            );

            var raw = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return (
                    Success: false,
                    Error: "Message sending failed."
                );
            }

            return (
                Success: true,
                Error: null
            );
        }
        catch (Exception ex)
        {
            return (
                Success: false,
                Error: $"Exception occurred: {ex.Message}"
            );
        }
    }
}
