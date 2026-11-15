
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using my_portfolio_mvc.Api.Interfaces;
using my_portfolio_mvc.Security.Captcha;
using my_portfolio_mvc.Services.Interfaces;
using my_portfolio_mvc.ViewModels.Contact;
using my_portfolio_mvc.ViewModels.Home;

namespace my_portfolio_mvc.Controllers;
public class HomeController : Controller
{
    private readonly IAuthApiClient _authApi;
    private readonly IProjectService _prjSrvc;
    private readonly ISkillService _skillSrvc;
    private readonly IContactService _contactSrvc;
    private readonly ITechnologyService _technologySrvc;

    private readonly ICaptchaService _captchaService;
    public HomeController(
        IProjectService prjSrvc, 
        ISkillService skillSrvc,
        IContactService contactSrvc,
        ITechnologyService technologySrvc,
        IAuthApiClient authApi,
        ICaptchaService captchaService)
    {
        _prjSrvc = prjSrvc;
        _skillSrvc = skillSrvc;
        _contactSrvc = contactSrvc;
        _technologySrvc = technologySrvc;
        _authApi = authApi;
        _captchaService = captchaService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Register()
    {
        return View(new UserRegisterViewModel { Email = "", Password = "", ConfirmPassword = "", FirstName="", LastName="" });
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(
    UserRegisterViewModel vm,
    CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(vm);

        var result = await _authApi.RegisterAsync(vm, cancellationToken);

        if (!result.Success)
        {
            ModelState.AddModelError(
                string.Empty,
                "Registration could not be completed. Please check your details or try again.");

            return View(vm);
        }

        return RedirectToAction(nameof(Login));
    }

    [AllowAnonymous]
    public async Task<IActionResult> AccessDenied()
    {
        return View();
    }

    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public async Task<IActionResult> Logout()
    //{
    //    // Call API logout first
    //    await _authApi.LogoutAsync();

    //    // Remove MVC auth cookie
    //    await HttpContext.SignOutAsync(
    //        CookieAuthenticationDefaults.AuthenticationScheme);

    //    return RedirectToAction("Login", "Home");
    //}

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout(
    CancellationToken cancellationToken)
    {
        try
        {
            await _authApi.LogoutAsync(cancellationToken);
        }
        finally
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
        }

        return RedirectToAction("Login", "Home");
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View(new LoginViewModel { Email = "", Password = "" });
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel vm, string? returnUrl = null, CancellationToken cancellationToken = default)
    {

        if (!ModelState.IsValid)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View(vm);
        }

        var res = await _authApi.LoginAsync(vm, cancellationToken);

        if (!res.Success)
        {
            ModelState.AddModelError("", "Invalid login attempt.");

            ViewData["ReturnUrl"] = returnUrl;

            return View(vm);
        }

        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return LocalRedirect(returnUrl);
        }

        return RedirectToAction("Index", "Admin");

    }

    public async Task<IActionResult> Index()
    {
        var vm = new HomeViewModel
        {
            FeaturedProjects = await _prjSrvc.GetFeaturedProjects(),
            Skills = await _skillSrvc.GetAllSkillsAsync(),
            Technologies = await _technologySrvc.GetAllTechnologies(),
            ContactMessage = new ContactMessageViewModel()
        };

        return View(vm);
    }

    public async Task<IActionResult> Privacy()
    {
        //return View();
        return NotFound();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SendContactMessage([FromBody] ContactMessageViewModel vm)
    {
        return StatusCode(
            StatusCodes.Status410Gone,
        new
        {
            success = false,
            error = "Contact message submissions are currently unavailable."
        });
        //////if (!ModelState.IsValid)
        //////{
        //////    var errors = ModelState
        //////        .Where(x => x.Value != null && x.Value.Errors.Count > 0)
        //////        .ToDictionary(
        //////            x => x.Key,
        //////            x => x.Value!.Errors
        //////                .Select(e => e.ErrorMessage)
        //////                .ToArray()
        //////        );

        //////    return BadRequest(new
        //////    {
        //////        success = false,
        //////        error = "Model validation failed.",
        //////        validationErrors = errors
        //////    });
        //////}

        ////////// Sanitize input
        ////////vm.Name = InputSanitizer.Sanitize(vm.Name);
        ////////vm.Email = InputSanitizer.Sanitize(vm.Email);
        ////////vm.Message = InputSanitizer.Sanitize(vm.Message);


        //////////UNCOMMENT BELOW FOR TROUBLESHOOTING
        //////// Console.WriteLine($"MVC CAPTCHA token exists: {!string.IsNullOrWhiteSpace(vm.CaptchaToken)}");

        ////////var client = _httpClientFactory.CreateClient("AuthApi");

        ////////var response = await client.PostAsJsonAsync(
        ////////    "api/contact/message/send",
        ////////    payload);

        ////////await _contactSrvc.SendContactMessageAsync(vm);

        ////////var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";


        ////////var token = Request.Form["cf-turnstile-response"];

        ////////var isCaptchaValid = await _captchaService.VerifyAsync(
        ////////   token!,
        ////////   HttpContext.Connection.RemoteIpAddress?.ToString());

        ////////if (!isCaptchaValid)
        ////////{
        ////////    ModelState.AddModelError("", "Captcha validation failed.");
        ////////    ViewData["ReturnUrl"] = returnUrl;
        ////////    return View(vm);
        ////////}

        //////var result = await _contactSrvc.SendContactMessageAsync(vm);

        //////if (!result.Success)
        //////{
        //////    return BadRequest(new
        //////    {
        //////        success = false,
        //////        error = result.Error
        //////    });
        //////}

        //////return Ok(new
        //////{
        //////    success = true,
        //////    message = "Message sent successfully."
        //////});
    }

    [HttpGet]
    [AllowAnonymous]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View();
    }

}