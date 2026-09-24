namespace my_portfolio_mvc.Infrastructure.Http;
public sealed class HttpLoggingOptions
{
    public bool LogResponseBody { get; set; } = false;
    public bool LogErrorResponseBody { get; set; } = true;
    public int MaxResponseBodyLength { get; set; } = 2000;
    public string[] ExcludedPaths { get; set; } =
    [
        "/api/auth/login",
        "/api/auth/refresh",
        "/api/auth/register"
    ];
}
