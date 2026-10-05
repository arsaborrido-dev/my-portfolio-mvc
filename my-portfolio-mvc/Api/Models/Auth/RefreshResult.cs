namespace my_portfolio_mvc.Api.Models.Auth;
public sealed class RefreshResult
{
    public bool Success { get; set; } = false;
    public string AccessToken { get; set; } = string.Empty;
}
