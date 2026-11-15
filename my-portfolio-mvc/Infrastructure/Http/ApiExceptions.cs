namespace my_portfolio_mvc.Infrastructure.Http;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public sealed class ApiExceptionFilter : IAsyncExceptionFilter
{
    public async Task OnExceptionAsync(ExceptionContext context)
    {
        var httpContext = context.HttpContext;

        if (context.Exception is ApiUnauthorizedException)
        {
            // Preserve the MVC cookie because another concurrent request
            // may already have successfully refreshed the session.
            var returnUrl =
                $"{httpContext.Request.Path}{httpContext.Request.QueryString}";

            context.Result = new RedirectToActionResult(
                "Login",
                "Home",
                new { returnUrl });

            context.ExceptionHandled = true;
            return;
        }

        if (context.Exception is ApiForbiddenException)
        {
            context.Result = new RedirectToActionResult(
                "AccessDenied",
                "Home",
                routeValues: null);

            context.ExceptionHandled = true;
        }
    }
}
public sealed class ApiUnauthorizedException : Exception
{
    public ApiUnauthorizedException()
        : base("The downstream API returned 401 Unauthorized.")
    {
    }
}

public sealed class ApiForbiddenException : Exception
{
    public ApiForbiddenException()
        : base("The downstream API returned 403 Forbidden.")
    {
    }
}