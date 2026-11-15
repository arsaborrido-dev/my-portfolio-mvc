
using System.Net.Http.Headers;

namespace my_portfolio_mvc.Infrastructure.Http;

public class AccessTokenHandler : DelegatingHandler
{
    private readonly ITokenStore _tokenStore;

    public AccessTokenHandler(ITokenStore tokenStore)
    {
        _tokenStore = tokenStore;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        // Preserve an explicitly supplied Authorization header.
        if (request.Headers.Authorization is null)
        {
            var token = await _tokenStore.GetAccessTokenAsync();

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }

        return await base.SendAsync(request, cancellationToken);
    }
}