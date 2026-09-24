
using Microsoft.AspNetCore.DataProtection;
using my_portfolio_mvc.Api.Clients;
using my_portfolio_mvc.Api.Interfaces;
using my_portfolio_mvc.Infrastructure.Http;
using my_portfolio_mvc.Security.Captcha;
using my_portfolio_mvc.Services.Implementations;
using my_portfolio_mvc.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

var authApiUrl = builder.Configuration["AuthApi:BaseUrl"] ?? "http://authapi:8080";

//this will avoid Data Protection key problem - Microsoft.AspNetCore.Antiforgery.AntiforgeryValidationException: The antiforgery token could not be decrypted.
builder.Services
    .AddDataProtection()
    .PersistKeysToFileSystem(
        new DirectoryInfo("/app/data-protection-keys"))
    .SetApplicationName("my_portfolio_mvc");

builder.Services.Configure<HttpLoggingOptions>(
    builder.Configuration.GetSection("HttpLogging"));

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<LoggingHandler>();

builder.Services.AddHttpClient<CaptchaService>()
    .AddHttpMessageHandler<LoggingHandler>();

//builder.Services.AddHttpClient<IAuthApiClient, AuthApiClient>(c =>
//{
//    c.BaseAddress = new Uri(authApiUrl);
//    c.Timeout = TimeSpan.FromSeconds(10);
//})
//    .AddHttpMessageHandler<LoggingHandler>();
////.AddHttpMessageHandler<TokenRefreshHandler>();

builder.Services.AddHttpClient<IProjectApiClient, ProjectApiClient>(client =>
{
    //client.BaseAddress = new Uri(builder.Configuration["AuthApi:BaseUrl"]!);
    client.BaseAddress = new Uri(authApiUrl);
    client.Timeout = TimeSpan.FromSeconds(10);
})
    .AddHttpMessageHandler<LoggingHandler>();

builder.Services.AddHttpClient<ISkillApiService, SkillApiClient>(client =>
{
    //client.BaseAddress = new Uri(builder.Configuration["AuthApi:BaseUrl"]!);
    client.BaseAddress = new Uri(authApiUrl);
    client.Timeout = TimeSpan.FromSeconds(10);
})
    .AddHttpMessageHandler<LoggingHandler>();

builder.Services.AddHttpClient<IContactApiClient, ContactApiClient>(client =>
{
    //client.BaseAddress = new Uri(builder.Configuration["AuthApi:BaseUrl"]!);
    client.BaseAddress = new Uri(authApiUrl);
    client.Timeout = TimeSpan.FromSeconds(10);
})
    .AddHttpMessageHandler<LoggingHandler>();

//builder.Services.AddHttpClient("AuthApi", c => {
//    c.BaseAddress = new Uri(authApiUrl);
//})
//    .AddHttpMessageHandler<LoggingHandler>();
//    //.AddHttpMessageHandler<TokenRefreshHandler>();

builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ISkillService, SkillService>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<ICaptchaService, CaptchaService>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
