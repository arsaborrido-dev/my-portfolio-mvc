using System.ComponentModel.DataAnnotations;

namespace my_portfolio_mvc.ViewModels.Contact;
public class ContactMessageViewModel
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(200)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(5000, MinimumLength = 1, ErrorMessage = "Message is too long.")]
    public string Message { get; set; } = string.Empty;

    [Required]
    [StringLength(2048)]
    public string CaptchaToken { get; set; } = string.Empty;

}
