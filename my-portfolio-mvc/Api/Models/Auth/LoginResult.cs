namespace my_portfolio_mvc.Api.Models.Auth;

public class LoginResult
{
    public bool Success { get; set; } = false;
    public string AccessToken { get; set; } = string.Empty;
}
