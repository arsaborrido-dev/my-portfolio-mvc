
using Microsoft.AspNetCore.DataProtection;
using my_portfolio_mvc.Api.Clients;
using my_portfolio_mvc.Api.Interfaces;
using my_portfolio_mvc.Api.Middleware;
using my_portfolio_mvc.Infrastructure.Http;
using my_portfolio_mvc.Security.Captcha;
using my_portfolio_mvc.Services.Implementations;
using my_portfolio_mvc.Services.Interfaces;


//-----------------------------------------------------------------------------------------//
//  SERVICES REGISTRATION
//_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_//

var builder = WebApplication.CreateBuilder(args);

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

//if (builder.Environment.IsDevelopment())
//{
//    // Allow HTTP for the local Docker network.
//    var isLocalDockerUrl =
//        authApiUri.Scheme == Uri.UriSchemeHttp &&
//        authApiUri.Host == "authapi" &&
//        authApiUri.Port == 8080;

//    if (!isLocalDockerUrl &&
//        authApiUri.Scheme != Uri.UriSchemeHttps)
//    {
//        throw new InvalidOperationException(
//            "AuthApi:BaseUrl must use HTTPS, except for " +
//            "the permitted local Docker URL.");
//    }
//}
//else if (authApiUri.Scheme != Uri.UriSchemeHttps)
//{
//    throw new InvalidOperationException(
//        "AuthApi:BaseUrl must use HTTPS outside Development.");
//}


if (builder.Environment.IsDevelopment())
{
    // Allow HTTP for the local Docker network.
    var isLocalDockerUrl =
        authApiUri.Scheme == Uri.UriSchemeHttp &&
        authApiUri.Host == "authapi" &&
        authApiUri.Port == 8080;

    if (!isLocalDockerUrl &&
        authApiUri.Scheme != Uri.UriSchemeHttps)
    {
        throw new InvalidOperationException(
            "AuthApi:BaseUrl must use HTTPS, except for " +
            "the permitted local Docker URL.");
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
}

builder.Services
    .AddDataProtection()
    .PersistKeysToFileSystem(
        new DirectoryInfo("/app/data-protection-keys"))
    .SetApplicationName("my_portfolio_mvc");

builder.Services.Configure<HttpLoggingOptions>(
    builder.Configuration.GetSection("HttpLogging"));

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<LoggingHandler>();
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
    .AddHttpMessageHandler<AutoRefreshHandler>();

builder.Services.AddHttpClient<ISkillApiService, SkillApiClient>(client =>
{
    //client.BaseAddress = new Uri(builder.Configuration["AuthApi:BaseUrl"]!);
    client.BaseAddress = new Uri(authApiUrl);
    client.Timeout = TimeSpan.FromSeconds(10);
})
    .AddHttpMessageHandler<LoggingHandler>()
    .AddHttpMessageHandler<AutoRefreshHandler>();

builder.Services.AddHttpClient<ITechnologyApiClient, TechnologyApiClient> (client =>
{
    //client.BaseAddress = new Uri(builder.Configuration["AuthApi:BaseUrl"]!);
    client.BaseAddress = new Uri(authApiUrl);
    client.Timeout = TimeSpan.FromSeconds(10);
})
    .AddHttpMessageHandler<LoggingHandler>()
    .AddHttpMessageHandler<AutoRefreshHandler>();

builder.Services.AddHttpClient<IContactApiClient, ContactApiClient>(client =>
{
    //client.BaseAddress = new Uri(builder.Configuration["AuthApi:BaseUrl"]!);
    client.BaseAddress = new Uri(authApiUrl);
    client.Timeout = TimeSpan.FromSeconds(10);
})
    .AddHttpMessageHandler<LoggingHandler>()
    .AddHttpMessageHandler<AutoRefreshHandler>();


//builder.Services.AddHttpClient("AuthApi", c => {
//    c.BaseAddress = new Uri(authApiUrl);
//})
//    .AddHttpMessageHandler<LoggingHandler>();
//    //.AddHttpMessageHandler<TokenRefreshHandler>();

//builder.Services.AddScoped<IAuthApiClient, AuthApiClient>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ISkillService, SkillService>();
builder.Services.AddScoped<ITechnologyService, TechnologyService>();
builder.Services.AddScoped<IContactService, ContactService>();
//builder.Services.AddScoped<ICaptchaService, CaptchaService>();

// Add services to the container.
builder.Services.AddControllersWithViews();

//-----------------------------------------------------------------------------------------//
//  PROGRAM PIPELINE
//_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_//

var app = builder.Build();

//check validators here (urls)
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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
