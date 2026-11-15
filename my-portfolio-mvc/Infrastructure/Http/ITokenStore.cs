namespace my_portfolio_mvc.Infrastructure.Http;

public interface ITokenStore
{
    Task<string?> GetAccessTokenAsync();
    Task<string?> GetRefreshTokenAsync();

    Task SetTokensAsync(
        string accessToken,
        string refreshToken,
        DateTimeOffset refreshTokenExpiresAtUtc);
}