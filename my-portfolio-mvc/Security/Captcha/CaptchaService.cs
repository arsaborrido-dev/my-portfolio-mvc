using my_portfolio_mvc.Security.Captcha.Model;
using System.Text.Json;

namespace my_portfolio_mvc.Security.Captcha;
public class CaptchaService : ICaptchaService
{
    private readonly HttpClient _httpClient;
    private readonly string _secretKey;

    public CaptchaService(
        HttpClient httpClient,
        IConfiguration config)
    {
        _httpClient = httpClient;

        _secretKey = config["Captcha:SecretKey"]
            ?? throw new InvalidOperationException(
                "Captcha:SecretKey is not configured.");
    }
    public async Task<bool> VerifyAsync(string token, string ip)
    {
        if (string.IsNullOrWhiteSpace(token))
            return false;

        var response = await _httpClient.PostAsync(
            "https://challenges.cloudflare.com/turnstile/v0/siteverify",
            new FormUrlEncodedContent(new[]
            {
            new KeyValuePair<string, string>(
                "secret",
                _secretKey),

            new KeyValuePair<string, string>(
                "response",
                token)            
            }));

        if (!response.IsSuccessStatusCode)
            return false;

        var json = await response.Content.ReadAsStringAsync();

        //Console.WriteLine("TURNSTILE RESPONSE: " + json);

        var result = JsonSerializer.Deserialize<TurnstileResponse>(
            json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        return result?.Success == true;
    }
}
