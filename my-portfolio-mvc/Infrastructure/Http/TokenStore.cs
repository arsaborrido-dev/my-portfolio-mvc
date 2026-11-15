

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace my_portfolio_mvc.Infrastructure.Http;

public class TokenStore : ITokenStore
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TokenStore(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<string?> GetAccessTokenAsync()
    {
        return await GetTokenAsync("access_token");
    }

    public async Task<string?> GetRefreshTokenAsync()
    {
        return await GetTokenAsync("refresh_token");
    }

    private async Task<string?> GetTokenAsync(string tokenName)
    {
        var context = _httpContextAccessor.HttpContext;

        if (context is null)
            return null;

        return await context.GetTokenAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            tokenName);
    }

    public async Task SetTokensAsync(
        string accessToken,
        string refreshToken,
        DateTimeOffset refreshTokenExpiresAtUtc)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(accessToken);
        ArgumentException.ThrowIfNullOrWhiteSpace(refreshToken);

        var context = _httpContextAccessor.HttpContext
            ?? throw new InvalidOperationException(
                "Cannot update tokens without an active HTTP context.");

        var authResult = await context.AuthenticateAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);

        if (!authResult.Succeeded || authResult.Principal is null)
        {
            throw new InvalidOperationException(
                "Cannot update tokens because the user is not authenticated.");
        }

        var properties = authResult.Properties
            ?? new AuthenticationProperties();

        var tokens = properties.GetTokens().ToList();

        SetToken(tokens, "access_token", accessToken);
        SetToken(tokens, "refresh_token", refreshToken);

        properties.ExpiresUtc = refreshTokenExpiresAtUtc;
        properties.IsPersistent = true;
        properties.StoreTokens(tokens);

        await context.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            authResult.Principal,
            properties);
    }

    private static void SetToken(
        List<AuthenticationToken> tokens,
        string name,
        string value)
    {
        var existingToken = tokens.FirstOrDefault(t => t.Name == name);

        if (existingToken is not null)
        {
            existingToken.Value = value;
        }
        else
        {
            tokens.Add(new AuthenticationToken
            {
                Name = name,
                Value = value
            });
        }
    }
}


//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Authentication.Cookies;

//namespace my_portfolio_mvc.Infrastructure.Http;

//public class TokenStore : ITokenStore
//{
//    private readonly IHttpContextAccessor _httpContextAccessor;

//    public TokenStore(IHttpContextAccessor httpContextAccessor)
//    {
//        _httpContextAccessor = httpContextAccessor;
//    }

//    public async Task<string?> GetAccessTokenAsync()
//    {
//        var context = _httpContextAccessor.HttpContext;

//        if (context is null)
//            return null;

//        return await context.GetTokenAsync(
//            CookieAuthenticationDefaults.AuthenticationScheme,
//            "access_token");
//    }

//    public async Task SetAccessTokenAsync(string accessToken)
//    {
//        ArgumentException.ThrowIfNullOrWhiteSpace(accessToken);

//        var context = _httpContextAccessor.HttpContext
//            ?? throw new InvalidOperationException(
//                "Cannot update the access token without an active HTTP context.");

//        var authResult = await context.AuthenticateAsync(
//            CookieAuthenticationDefaults.AuthenticationScheme);

//        if (!authResult.Succeeded || authResult.Principal is null)
//        {
//            throw new InvalidOperationException(
//                "Cannot update access token because the user is not authenticated.");
//        }

//        var properties = authResult.Properties
//            ?? new AuthenticationProperties();

//        var tokens = properties.GetTokens().ToList();

//        var existingToken = tokens.FirstOrDefault(
//            t => t.Name == "access_token");

//        if (existingToken is not null)
//        {
//            existingToken.Value = accessToken;
//        }
//        else
//        {
//            tokens.Add(new AuthenticationToken
//            {
//                Name = "access_token",
//                Value = accessToken
//            });
//        }

//        properties.StoreTokens(tokens);

//        await context.SignInAsync(
//            CookieAuthenticationDefaults.AuthenticationScheme,
//            authResult.Principal,
//            properties);
//    }
//}
