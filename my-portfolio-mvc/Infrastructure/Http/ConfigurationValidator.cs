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

        //var isHttps = uri.Scheme == Uri.UriSchemeHttps;

        //// Permit HTTP only for the expected local Docker endpoint.
        //var isLocalDevelopmentHttp =
        //    environment.IsDevelopment() &&
        //    uri.Scheme == Uri.UriSchemeHttp &&
        //    uri.Host == "authapi" &&
        //    uri.Port == 8080;

        //if (!isHttps && !isLocalDevelopmentHttp)
        //{
        //    logger.LogError(
        //        "Invalid URL scheme for configuration key '{Key}'.",
        //        key);

        //    throw new InvalidOperationException(
        //        $"{key} must use HTTPS, except for the permitted local Development endpoint.");
        //}

        var isHttps = uri.Scheme == Uri.UriSchemeHttps;

        // Permit HTTP for the expected local Docker endpoint.
        var isLocalDevelopmentHttp =
            environment.IsDevelopment() &&
            uri.Scheme == Uri.UriSchemeHttp &&
            uri.Host.Equals(
                "authapi",
                StringComparison.OrdinalIgnoreCase) &&
            uri.Port == 8080;

        // Permit HTTP for the expected Railway private endpoint.
        var isRailwayPrivateHttp =
            !environment.IsDevelopment() &&
            uri.Scheme == Uri.UriSchemeHttp &&
            uri.Host.Equals(
                "threedeeeeecentralauthservice.railway.internal",
                StringComparison.OrdinalIgnoreCase) &&
            uri.Port == 8080;

        if (!isHttps &&
            !isLocalDevelopmentHttp &&
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
