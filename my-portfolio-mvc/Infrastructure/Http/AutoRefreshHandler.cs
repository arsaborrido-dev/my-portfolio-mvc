
using my_portfolio_mvc.Api.Interfaces;
using my_portfolio_mvc.Infrastructure.Http;
using System.Net;
using System.Net.Http.Headers;

namespace my_portfolio_mvc.Api.Middleware;

public class AutoRefreshHandler : DelegatingHandler
{
    private readonly IAuthApiClient _authApiClient;
    private readonly ITokenStore _tokenStore;
    private readonly IRefreshCoordinator _refreshCoordinator;

    public AutoRefreshHandler(
        IAuthApiClient authApiClient,
        ITokenStore tokenStore,
        IRefreshCoordinator refreshCoordinator)
    {
        _authApiClient = authApiClient;
        _tokenStore = tokenStore;
        _refreshCoordinator = refreshCoordinator;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(
            request,
            cancellationToken);

        // Only attempt refresh for requests that used a Bearer token.
        if (response.StatusCode != HttpStatusCode.Unauthorized ||
            !string.Equals(
                request.Headers.Authorization?.Scheme,
                "Bearer",
                StringComparison.OrdinalIgnoreCase))
        {
            return response;
        }

        //var refreshResult = await _authApiClient.RefreshAsync(
        //    cancellationToken);

        var currentRefreshToken = await _tokenStore.GetRefreshTokenAsync();

        if (string.IsNullOrWhiteSpace(currentRefreshToken))
        {
            return response;
        }

        var refreshResult = await _refreshCoordinator.RefreshAsync(
            currentRefreshToken,
            ct => _authApiClient.RefreshAsync(ct),
            cancellationToken);

        // Preserve the current session if refresh fails.
        // A rejection may be caused by another concurrent request
        // successfully rotating the refresh token.
        if (!refreshResult.Success)
        {
            return response;
        }

        await _tokenStore.SetTokensAsync(
            refreshResult.AccessToken,
            refreshResult.RefreshToken,
            refreshResult.RefreshTokenExpiresAtUtc);

        HttpContent? retryContent = null;

        if (request.Content is not null)
        {
            var contentBytes = await request.Content.ReadAsByteArrayAsync(
                cancellationToken);

            retryContent = new ByteArrayContent(contentBytes);

            foreach (var header in request.Content.Headers)
            {
                retryContent.Headers.TryAddWithoutValidation(
                    header.Key,
                    header.Value);
            }
        }

        var retryRequest = new HttpRequestMessage(
            request.Method,
            request.RequestUri)
        {
            Version = request.Version,
            VersionPolicy = request.VersionPolicy,
            Content = retryContent
        };

        foreach (var header in request.Headers)
        {
            if (!string.Equals(
                header.Key,
                "Authorization",
                StringComparison.OrdinalIgnoreCase))
            {
                retryRequest.Headers.TryAddWithoutValidation(
                    header.Key,
                    header.Value);
            }
        }

        foreach (var option in request.Options)
        {
            retryRequest.Options.Set(
                new HttpRequestOptionsKey<object?>(option.Key),
                option.Value);
        }

        retryRequest.Headers.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                refreshResult.AccessToken);

        response.Dispose();

        using (retryRequest)
        {
            return await base.SendAsync(
                retryRequest,
                cancellationToken);
        }
    }
}
