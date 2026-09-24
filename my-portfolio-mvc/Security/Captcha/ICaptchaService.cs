namespace my_portfolio_mvc.Security.Captcha
{
    public interface ICaptchaService
    {
        public Task<bool> VerifyAsync(string token, string ip);
    }
}
