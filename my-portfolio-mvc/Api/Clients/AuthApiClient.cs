using my_portfolio_mvc.Api.Interfaces; 
using my_portfolio_mvc.Api.Models.Auth;

namespace my_portfolio_mvc.Api.Clients;

public class AuthApiClient : IAuthApiClient
{
    private readonly HttpClient _http;

    public AuthApiClient
        (HttpClient http)
    {
        _http = http;
    }

    public async Task<RefreshResult> RefreshAsync(CancellationToken cancellationToken)
    {
        var response = await _http.PostAsync("api/auth/refresh", null, cancellationToken);

        if (!response.IsSuccessStatusCode)
            return new RefreshResult { Success = false };

        var json = await response.Content.ReadFromJsonAsync<RefreshResponseDto>();

        if (json?.AccessToken is null)
            return new RefreshResult { Success = false };

        return new RefreshResult
        {
            Success = true,
            AccessToken = json.AccessToken
        };
    }
}
