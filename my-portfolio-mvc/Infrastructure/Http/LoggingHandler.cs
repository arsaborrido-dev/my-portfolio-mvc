

using Microsoft.Extensions.Options;

namespace my_portfolio_mvc.Infrastructure.Http;
public class LoggingHandler : DelegatingHandler
{
    private readonly ILogger<LoggingHandler> _logger;
    private readonly HttpLoggingOptions _options;
    public LoggingHandler(ILogger<LoggingHandler> logger, IOptions<HttpLoggingOptions> options)
    {
        _logger = logger;
        _options = options.Value;
    }
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
    {
        // DON'T read request.Content - it breaks the stream
        _logger.LogInformation("-> {Method} {Url}", request.Method, request.RequestUri);

        var response = await base.SendAsync(request, ct);

        var shouldLogBody = ShouldLogResponseBody(request, response);

        // Only log body if not success or debug
        if (!response.IsSuccessStatusCode)
        {

            if (shouldLogBody)
            {
                var body = await ReadResponseBodyAsync(response, ct);

                _logger.LogWarning(
                    "<- {Status} {Url} Body: {Body}",
                    (int)response.StatusCode,
                    request.RequestUri,
                    body);
            }
            else
            {
                _logger.LogWarning(
                    "<- {Status} {Url}",
                    (int)response.StatusCode,
                    request.RequestUri);
            }
        }
        else
        {
            if (shouldLogBody)
            {
                var body = await ReadResponseBodyAsync(response, ct);

                _logger.LogInformation(
                    "<- {Status} {Url} Body: {Body}",
                    (int)response.StatusCode,
                    request.RequestUri,
                    body);
            }
            else
            {
                _logger.LogInformation(
                    "<- {Status} {Url}",
                    (int)response.StatusCode,
                    request.RequestUri);
            }
            
        }

        return response;
    }
    private bool ShouldLogResponseBody(
        HttpRequestMessage request,
        HttpResponseMessage response)
    {
        var path = request.RequestUri?.AbsolutePath;

        if (string.IsNullOrWhiteSpace(path))
        {
            return false;
        }

        // Never log sensitive authentication responses.
        if (_options.ExcludedPaths.Any(
            excluded => path.Equals(
                excluded,
                StringComparison.OrdinalIgnoreCase)))
        {
            return false;
        }

        if (response.IsSuccessStatusCode)
        {
            return _options.LogResponseBody;
        }

        return _options.LogErrorResponseBody;
    }   

    private async Task<string> ReadResponseBodyAsync(
    HttpResponseMessage response,
    CancellationToken ct)
    {
        if (response.Content == null)
        {
            return string.Empty;
        }

        var body = await response.Content.ReadAsStringAsync(ct);

        if (body.Length <= _options.MaxResponseBodyLength)
        {
            return body;
        }

        return body[.._options.MaxResponseBodyLength]
            + "... [truncated]";
    }    
}
