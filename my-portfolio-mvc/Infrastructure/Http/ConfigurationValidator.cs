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

        // Permit HTTP only for the expected local Docker endpoint.
        var isLocalDevelopmentHttp =
            environment.IsDevelopment() &&
            uri.Scheme == Uri.UriSchemeHttp &&
            uri.Host == "authapi" &&
            uri.Port == 8080;

        if (!isHttps && !isLocalDevelopmentHttp)
        {
            logger.LogError(
                "Invalid URL scheme for configuration key '{Key}'.",
                key);

            throw new InvalidOperationException(
                $"{key} must use HTTPS, except for the permitted local Development endpoint.");
        }

        logger.LogInformation(
            "Validated AuthApi base URL configuration.");
    }
}
