

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

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken ct)
    {
        var path = request.RequestUri?.AbsolutePath ?? "(unknown path)";

        _logger.LogInformation(
            "LoggingHandler: BEFORE base.SendAsync - {Method} {Path}",
            request.Method,
            path);

        var response = await base.SendAsync(request, ct);

        _logger.LogInformation(
            "LoggingHandler: AFTER base.SendAsync - Status: {StatusCode}",
            (int)response.StatusCode);

        var shouldLogBody = ShouldLogResponseBody(request, response);

        if (!response.IsSuccessStatusCode)
        {
            if (shouldLogBody)
            {
                var body = await ReadResponseBodyAsync(response, ct);

                _logger.LogWarning(
                    "<- {Status} {Path} Body: {Body}",
                    (int)response.StatusCode,
                    path,
                    body);
            }
            else
            {
                _logger.LogWarning(
                    "<- {Status} {Path}",
                    (int)response.StatusCode,
                    path);
            }
        }
        else
        {
            if (shouldLogBody)
            {
                var body = await ReadResponseBodyAsync(response, ct);

                _logger.LogInformation(
                    "<- {Status} {Path} Body: {Body}",
                    (int)response.StatusCode,
                    path,
                    body);
            }
            else
            {
                _logger.LogInformation(
                    "<- {Status} {Path}",
                    (int)response.StatusCode,
                    path);
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
