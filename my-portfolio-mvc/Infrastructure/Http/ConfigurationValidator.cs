namespace my_portfolio_mvc.Infrastructure.Http;


public class ConfigurationValidator
{
    public static void Validate(
        IConfiguration config,
        ILogger logger,
        IHostEnvironment environment)
    {
        const string key = "AuthApi:BaseUrl";
        var value = config[key];

        if (!Uri.TryCreate(value, UriKind.Absolute, out var uri))
        {
            logger.LogError(
                "Invalid URL for configuration key '{Key}'.",
                key);

            throw new InvalidOperationException(
                $"{key} must be a valid absolute URL.");
        }

        var isHttps = uri.Scheme == Uri.UriSchemeHttps;

        // MVC running locally in Visual Studio, calling AuthApi in Docker.
        var isLocalHostDevelopmentHttp =
            environment.IsDevelopment() &&
            uri.Scheme == Uri.UriSchemeHttp &&
            (uri.Host.Equals(
                "localhost",
                StringComparison.OrdinalIgnoreCase) ||
             uri.Host.Equals(
                "127.0.0.1",
                StringComparison.OrdinalIgnoreCase)) &&
            uri.Port == 8081;

        // MVC and AuthApi communicating inside Docker Compose.
        var isLocalDockerDevelopmentHttp =
            environment.IsDevelopment() &&
            uri.Scheme == Uri.UriSchemeHttp &&
            uri.Host.Equals(
                "authapi",
                StringComparison.OrdinalIgnoreCase) &&
            uri.Port == 8080;

        // AuthApi's private Railway endpoint.
        var isRailwayPrivateHttp =
            !environment.IsDevelopment() &&
            uri.Scheme == Uri.UriSchemeHttp &&
            uri.Host.Equals(
                "threedeeeeecentralauthservice.railway.internal",
                StringComparison.OrdinalIgnoreCase) &&
            uri.Port == 8080;

        if (!isHttps &&
            !isLocalHostDevelopmentHttp &&
            !isLocalDockerDevelopmentHttp &&
            !isRailwayPrivateHttp)
        {
            logger.LogError(
                "Invalid URL scheme or endpoint for configuration key '{Key}'.",
                key);

            throw new InvalidOperationException(
                $"{key} must use HTTPS, except for the permitted " +
                "local Development and Railway private endpoints.");
        }

        logger.LogInformation(
            "Validated AuthApi base URL configuration.");
    }
}