
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using my_portfolio_mvc.Api.Interfaces;
using my_portfolio_mvc.Api.Models.Auth;
using my_portfolio_mvc.Infrastructure.Http;
using my_portfolio_mvc.ViewModels.Home;
using System.Security.Claims;

namespace my_portfolio_mvc.Api.Clients;

public class AuthApiClient : IAuthApiClient
{
    private readonly HttpClient _http;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITokenStore _tokenStore;
    public AuthApiClient(
        HttpClient http,
        IHttpContextAccessor httpContextAccessor,
        ITokenStore tokenStore)
    {
        _http = http;
        _httpContextAccessor = httpContextAccessor;
        _tokenStore = tokenStore;
    }

    public async Task<RegisterResult> RegisterAsync(UserRegisterViewModel vm, CancellationToken cancellationToken)
    {

        var regDto = new RegisterDto
        {
            FirstName = vm.FirstName,
            LastName = vm.LastName,
            Password = vm.Password,
            Email = vm.Email
        };

        var response = await _http.PostAsJsonAsync(
            "api/auth/register",
            regDto,
            cancellationToken);

        if (!response.IsSuccessStatusCode)
            return new RegisterResult { Success = false };

        //TEMPORARY
        return new RegisterResult
        {
            Success = true
        };
    }
    public async Task<LoginResult> LoginAsync(LoginViewModel vm, CancellationToken cancellationToken)
    {

        var loginDto = new LoginDto
        {
            Email = vm.Email.Trim(),
            Password = vm.Password
        };

        var response = await _http.PostAsJsonAsync(
            "api/auth/login",
            loginDto,
            cancellationToken);

        if (!response.IsSuccessStatusCode)
            return new LoginResult { Success = false };

        var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponseDto>(
            cancellationToken: cancellationToken);

        if (string.IsNullOrWhiteSpace(loginResponse?.AccessToken))
        {
            return new LoginResult { Success = false };
        }

        if (string.IsNullOrWhiteSpace(loginResponse.RefreshToken))
        {
            return new LoginResult { Success = false };
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, loginResponse.UserId),
            new Claim(ClaimTypes.Email, vm.Email)
        };

        var identity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme);

        var principal = new ClaimsPrincipal(identity);

        var context = _httpContextAccessor.HttpContext!;

        var properties = new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = loginResponse.RefreshTokenExpiresAtUtc
        };

        properties.StoreTokens(new[]
        {
            new AuthenticationToken
            {
                Name = "access_token",
                Value = loginResponse.AccessToken
            },
            new AuthenticationToken
            {
                Name = "refresh_token",
                Value = loginResponse.RefreshToken
            }
        });

        //store token in to MVC authentication cookie
        await context.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            properties);


        //TEMPORARY
        return new LoginResult
        {
            Success = true
        };
    }
    public async Task<RefreshResult> RefreshAsync(
        CancellationToken cancellationToken)
    {
        var refreshToken = await _tokenStore.GetRefreshTokenAsync();

        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            return new RefreshResult
            {
                Success = false,
                FailureReason = RefreshFailureReason.Rejected
            };
        }

        using var request = new HttpRequestMessage(
            HttpMethod.Post,
            "api/auth/refresh");

        request.Headers.Add(
            "Cookie",
            $"refreshToken={refreshToken}");

        HttpResponseMessage response;

        try
        {
            response = await _http.SendAsync(
                request,
                cancellationToken);
        }
        catch (HttpRequestException)
        {
            return new RefreshResult
            {
                Success = false,
                FailureReason = RefreshFailureReason.Unavailable
            };
        }
        catch (TaskCanceledException) when (!cancellationToken.IsCancellationRequested)
        {
            return new RefreshResult
            {
                Success = false,
                FailureReason = RefreshFailureReason.Unavailable
            };
        }

        using (response)
        {
            if (response.StatusCode ==
                    System.Net.HttpStatusCode.Unauthorized ||
                response.StatusCode ==
                    System.Net.HttpStatusCode.Forbidden)
            {
                return new RefreshResult
                {
                    Success = false,
                    FailureReason = RefreshFailureReason.Rejected
                };
            }

            if (!response.IsSuccessStatusCode)
            {
                return new RefreshResult
                {
                    Success = false,
                    FailureReason = RefreshFailureReason.Unavailable
                };
            }

            RefreshResponseDto? json;

            try
            {
                json = await response.Content.ReadFromJsonAsync<RefreshResponseDto>(
                    cancellationToken: cancellationToken);
            }
            catch (System.Text.Json.JsonException)
            {
                return new RefreshResult
                {
                    Success = false,
                    FailureReason = RefreshFailureReason.Unavailable
                };
            }

            if (string.IsNullOrWhiteSpace(json?.AccessToken) ||
                string.IsNullOrWhiteSpace(json.RefreshToken))
            {
                return new RefreshResult
                {
                    Success = false,
                    FailureReason = RefreshFailureReason.Unavailable
                };
            }

            return new RefreshResult
            {
                Success = true,
                FailureReason = RefreshFailureReason.None,
                AccessToken = json.AccessToken,
                AccessTokenExpiresAtUtc = json.AccessTokenExpiresAtUtc,
                RefreshToken = json.RefreshToken,
                RefreshTokenExpiresAtUtc = json.RefreshTokenExpiresAtUtc
            };
        }
    }
    public async Task LogoutAsync(
        CancellationToken cancellationToken = default)
    {
        var refreshToken = await _tokenStore.GetRefreshTokenAsync();

        if (string.IsNullOrWhiteSpace(refreshToken))
            return;

        using var request = new HttpRequestMessage(
            HttpMethod.Post,
            "api/auth/revoke");

        request.Headers.Add(
            "Cookie",
            $"refreshToken={refreshToken}");

        using var response = await _http.SendAsync(
            request,
            cancellationToken);

        response.EnsureSuccessStatusCode();
    }

}
