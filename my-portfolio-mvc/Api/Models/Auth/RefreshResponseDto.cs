namespace my_portfolio_mvc.Api.Models.Auth;

public sealed class RefreshResponseDto
{
    public string AccessToken = string.Empty;
    public DateTimeOffset AccessTokenExpiresAtUtc;
    public string RefreshToken = string.Empty;
    public DateTimeOffset RefreshTokenExpiresAtUtc;
}
