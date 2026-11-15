namespace my_portfolio_mvc.Api.Models.Auth;

public sealed class LoginResponseDto
{
    public string AccessToken { get; set; } = string.Empty;
    public DateTimeOffset AccessTokenExpiresAtUtc { get; set; }
    public string RefreshToken { get; set; } = string.Empty;
    public DateTimeOffset RefreshTokenExpiresAtUtc { get; set; }
    public string UserId { get; set; } = string.Empty;
}