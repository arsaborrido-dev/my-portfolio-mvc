using my_portfolio_mvc.Api.Interfaces;
using System.Net;
using System.Net.Http.Headers;

namespace my_portfolio_mvc.Api.Middleware;
public class AutoRefreshHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAuthApiClient _authApiClient;

    public AutoRefreshHandler(
        IHttpContextAccessor httpContextAccessor,
        IAuthApiClient authApiClient)
    {
        _httpContextAccessor = httpContextAccessor;
        _authApiClient = authApiClient;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        // 1. Send the original request
        var response = await base.SendAsync(request, cancellationToken);

        // 2. If unauthorized → try refresh
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            var refreshResult = await _authApiClient.RefreshAsync(cancellationToken);

            if (!refreshResult.Success)
                return response; // refresh failed → return 401

            // 3. Retry original request with new access token
            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", refreshResult.AccessToken);

            return await base.SendAsync(request, cancellationToken);
        }

        return response;
    }
}
