namespace my_portfolio_mvc.Security.Captcha.Model;
public class TurnstileResponse
{
    public bool Success { get; set; }
    public string[]? ErrorCodes { get; set; }
    public string? Hostname { get; set; }
    public DateTime? ChallengeTs { get; set; }
}
