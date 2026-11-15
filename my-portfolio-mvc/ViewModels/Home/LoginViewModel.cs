using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace my_portfolio_mvc.ViewModels.Home;

public class LoginViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    //[JsonPropertyName("cf-turnstile-response")]
    //public string? CaptchaToken { get; set; } = string.Empty;
}
