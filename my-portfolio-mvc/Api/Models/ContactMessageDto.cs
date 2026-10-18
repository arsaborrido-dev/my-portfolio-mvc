
namespace my_portfolio_mvc.Api.Models;
public class ContactMessageDto
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string CaptchaToken { get; set; } = string.Empty;
}
