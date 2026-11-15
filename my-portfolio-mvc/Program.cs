
using Microsoft.AspNetCore.DataProtection;
using my_portfolio_mvc.Api.Clients;
using my_portfolio_mvc.Api.Interfaces;
using my_portfolio_mvc.Api.Middleware;
using my_portfolio_mvc.Infrastructure.Http;
using my_portfolio_mvc.Security.Captcha;
using my_portfolio_mvc.Services.Implementations;
using my_portfolio_mvc.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using my_portfolio_mvc.Infrastructure.DataProtection;

//-----------------------------------------------------------------------------------------//
//  SERVICES REGISTRATION
//_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_//

var builder = WebApplication.CreateBuilder(args);

//var dataProtectionConnectionString =
//    builder.Configuration["DataProtection:ConnectionString"];
var dataProtectionConnectionString =
    builder.Environment.IsDevelopment()
        ? builder.Configuration["DataProtection:LocalConnectionString"]
        : builder.Configuration["DataProtection:ConnectionString"];

if (string.IsNullOrWhiteSpace(dataProtectionConnectionString))
{
    throw new InvalidOperationException(
        builder.Environment.IsDevelopment()
            ? "Missing configuration: DataProtection:LocalConnectionString."
            : "Missing configuration: DataProtection:ConnectionString.");
}

//if (string.IsNullOrWhiteSpace(dataProtectionConnectionString))
//{
//    throw new InvalidOperationException(
//        "Missing required configuration: DataProtection:ConnectionString.");
//}

//var authApiUrl = builder.Configuration["AuthApi:BaseUrl"] ?? "http://authapi:8080";
var authApiUrl = builder.Configuration["AuthApi:BaseUrl"];

if (string.IsNullOrWhiteSpace(authApiUrl))
{
    if (builder.Environment.IsDevelopment())
    {
        authApiUrl = "http://authapi:8080";
    }
    else
    {
        throw new InvalidOperationException(
            "Missing required configuration: AuthApi:BaseUrl.");
    }
}

if (!Uri.TryCreate(authApiUrl, UriKind.Absolute, out var authApiUri))
{
    throw new InvalidOperationException(
        "AuthApi:BaseUrl must be a valid absolute URL.");
}

//builder.Services
//    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//    .AddCookie(options =>
//    {
//        options.LoginPath = "/Home/Login";
//        options.AccessDeniedPath = "/Home/AccessDenied";
//    });

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Home/Login";
        options.AccessDeniedPath = "/Home/AccessDenied";

        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Lax;

        // Match the AuthApi refresh-token lifetime.
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
        options.SlidingExpiration = false;
    });

if (builder.Environment.IsDevelopment())
{
    // Allow HTTP for local development:
    // 1. MVC running in Visual Studio, calling AuthApi in Docker.
    // 2. MVC and AuthApi communicating within Docker Compose.
    var isLocalDockerUrl =
        authApiUri.Scheme == Uri.UriSchemeHttp &&
        authApiUri.Host == "authapi" &&
        authApiUri.Port == 8080;

    var isLocalHostUrl =
        authApiUri.Scheme == Uri.UriSchemeHttp &&
        (authApiUri.Host == "localhost" ||
         authApiUri.Host == "127.0.0.1") &&
        authApiUri.Port == 8081;

    if (!isLocalDockerUrl &&
        !isLocalHostUrl &&
        authApiUri.Scheme != Uri.UriSchemeHttps)
    {
        throw new InvalidOperationException(
            "AuthApi:BaseUrl must use HTTPS, except for " +
            "permitted local Development endpoints.");
    }
}
else
{

    // Allow HTTP only for the known Railway private service.
    var isRailwayPrivateUrl =
        authApiUri.Scheme == Uri.UriSchemeHttp &&
        authApiUri.Host.Equals(
            "threedeeeeecentralauthservice.railway.internal",
            StringComparison.OrdinalIgnoreCase) &&
        authApiUri.Port == 8080;

    if (!isRailwayPrivateUrl &&
        authApiUri.Scheme != Uri.UriSchemeHttps)
    {
        throw new InvalidOperationException(
            "AuthApi:BaseUrl must use HTTPS, except for " +
            "the permitted Railway private service URL.");
    }

    ////// Require HTTPS outside Development.
    ////if (authApiUri.Scheme != Uri.UriSchemeHttps)
    ////{
    ////    throw new InvalidOperationException(
    ////        "AuthApi:BaseUrl must use HTTPS outside Development.");
    ////}
}


////builder.Services
////    .AddDataProtection()
////    .PersistKeysToFileSystem(
////        new DirectoryInfo("/app/data-protection-keys"))
////    .SetApplicationName("my_portfolio_mvc");
///

builder.Services.AddDbContext<DataProtectionKeyContext>(options =>
    options.UseNpgsql(
        dataProtectionConnectionString,
        npgsqlOptions => npgsqlOptions.MigrationsHistoryTable(
            "__DataProtectionMigrationsHistory")));

builder.Services
    .AddDataProtection()
    .PersistKeysToDbContext<DataProtectionKeyContext>()
    .SetApplicationName("my_portfolio_mvc");

builder.Services.Configure<HttpLoggingOptions>(
    builder.Configuration.GetSection("HttpLogging"));

builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IRefreshCoordinator, RefreshCoordinator>();

builder.Services.AddTransient<LoggingHandler>();
builder.Services.AddTransient<AccessTokenHandler>();
builder.Services.AddTransient<AutoRefreshHandler>();

//builder.Services.AddHttpClient<CaptchaService>()
//    .AddHttpMessageHandler<LoggingHandler>();
builder.Services.AddHttpClient<ICaptchaService, CaptchaService>()
    .AddHttpMessageHandler<LoggingHandler>();

builder.Services.AddHttpClient<IAuthApiClient, AuthApiClient>(client =>
{
    client.BaseAddress = new Uri(authApiUrl);
    client.Timeout = TimeSpan.FromSeconds(10);
})
    .AddHttpMessageHandler<LoggingHandler>();

builder.Services.AddHttpClient<IProjectApiClient, ProjectApiClient>(client =>
{
    //client.BaseAddress = new Uri(builder.Configuration["AuthApi:BaseUrl"]!);
    client.BaseAddress = new Uri(authApiUrl);
    client.Timeout = TimeSpan.FromSeconds(10);
})
    .AddHttpMessageHandler<LoggingHandler>()
    .AddHttpMessageHandler<AccessTokenHandler>()
    .AddHttpMessageHandler<AutoRefreshHandler>();

builder.Services.AddHttpClient<ISkillApiService, SkillApiClient>(client =>
{
    //client.BaseAddress = new Uri(builder.Configuration["AuthApi:BaseUrl"]!);
    client.BaseAddress = new Uri(authApiUrl);
    client.Timeout = TimeSpan.FromSeconds(10);
})
    .AddHttpMessageHandler<LoggingHandler>()
    .AddHttpMessageHandler<AccessTokenHandler>()
    .AddHttpMessageHandler<AutoRefreshHandler>();

builder.Services.AddHttpClient<ITechnologyApiClient, TechnologyApiClient> (client =>
{
    //client.BaseAddress = new Uri(builder.Configuration["AuthApi:BaseUrl"]!);
    client.BaseAddress = new Uri(authApiUrl);
    client.Timeout = TimeSpan.FromSeconds(10);
})
    .AddHttpMessageHandler<LoggingHandler>()
    .AddHttpMessageHandler<AccessTokenHandler>()
    .AddHttpMessageHandler<AutoRefreshHandler>();


builder.Services.AddHttpClient<IContactApiClient, ContactApiClient>(client =>
{
    //client.BaseAddress = new Uri(builder.Configuration["AuthApi:BaseUrl"]!);
    client.BaseAddress = new Uri(authApiUrl);
    client.Timeout = TimeSpan.FromSeconds(10);
})
    .AddHttpMessageHandler<LoggingHandler>();
    //.AddHttpMessageHandler<AccessTokenHandler>()
    //.AddHttpMessageHandler<AutoRefreshHandler>();

builder.Services.AddHttpClient<IAddressApiClient, AddressApiClient>(client =>
{
    client.BaseAddress = new Uri(authApiUrl);
    client.Timeout = TimeSpan.FromSeconds(10);
})
    .AddHttpMessageHandler<LoggingHandler>()
    .AddHttpMessageHandler<AccessTokenHandler>()
    .AddHttpMessageHandler<AutoRefreshHandler>();

builder.Services.AddHttpClient<IPhoneApiClient, PhoneApiClient>(client =>
{
    client.BaseAddress = new Uri(authApiUrl);
    client.Timeout = TimeSpan.FromSeconds(10);
})
    .AddHttpMessageHandler<LoggingHandler>()
    .AddHttpMessageHandler<AccessTokenHandler>()
    .AddHttpMessageHandler<AutoRefreshHandler>();

//builder.Services.AddHttpClient("AuthApi", c => {
//    c.BaseAddress = new Uri(authApiUrl);
//})
//    .AddHttpMessageHandler<LoggingHandler>();
//    //.AddHttpMessageHandler<TokenRefreshHandler>();

builder.Services.AddScoped<ApiExceptionFilter>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ISkillService, SkillService>();
builder.Services.AddScoped<ITechnologyService, TechnologyService>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<ITokenStore, TokenStore>();
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IPhoneService, PhoneService>();

builder.Services.AddHealthChecks();

// Add services to the container
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.AddService<ApiExceptionFilter>();
});

//-----------------------------------------------------------------------------------------//
//  PROGRAM PIPELINE
//_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_//

var app = builder.Build();

//check/confirm environment
app.Logger.LogInformation(
    "Environment name: {EnvironmentName}",
    app.Environment.EnvironmentName);

////check validators here (urls)
var logger = app.Services.GetRequiredService<ILogger<Program>>();
ConfigurationValidator.Validate(builder.Configuration, logger, app.Environment);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapHealthChecks("/health");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
