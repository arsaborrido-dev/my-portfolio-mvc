
using Microsoft.AspNetCore.Mvc;

using my_portfolio_mvc.Security;
using my_portfolio_mvc.Services.Interfaces;
using my_portfolio_mvc.ViewModels.Contact;
using my_portfolio_mvc.ViewModels.Home;

namespace my_portfolio_mvc.Controllers;
public class HomeController : Controller
{
    private readonly IProjectService _prjSrvc;
    private readonly ISkillService _skillSrvc;
    private readonly IContactService _contactSrvc;
    private readonly ITechnologyService _technologySrvc;
    public HomeController(
        IProjectService prjSrvc, 
        ISkillService skillSrvc,
        IContactService contactSrvc,
        ITechnologyService technologySrvc)
    {
        _prjSrvc = prjSrvc;
        _skillSrvc = skillSrvc;
        _contactSrvc = contactSrvc;
        _technologySrvc = technologySrvc;
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
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SendContactMessage([FromBody] ContactMessageViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value != null && x.Value.Errors.Count > 0)
                .ToDictionary(
                    x => x.Key,
                    x => x.Value!.Errors
                        .Select(e => e.ErrorMessage)
                        .ToArray()
                );

            return BadRequest(new
            {
                success = false,
                error = "Model validation failed.",
                validationErrors = errors
            });
        }

        //// Sanitize input
        //vm.Name = InputSanitizer.Sanitize(vm.Name);
        //vm.Email = InputSanitizer.Sanitize(vm.Email);
        //vm.Message = InputSanitizer.Sanitize(vm.Message);
                   

        ////UNCOMMENT BELOW FOR TROUBLESHOOTING
        // Console.WriteLine($"MVC CAPTCHA token exists: {!string.IsNullOrWhiteSpace(vm.CaptchaToken)}");

        //var client = _httpClientFactory.CreateClient("AuthApi");

        //var response = await client.PostAsJsonAsync(
        //    "api/contact/message/send",
        //    payload);

        //await _contactSrvc.SendContactMessageAsync(vm);

        //var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        var result = await _contactSrvc.SendContactMessageAsync(vm);
        
        if (!result.Success)
        {
            return BadRequest(new
            {
                success = false,
                error = result.Error
            });
        }

        return Ok(new
        {
            success = true,
            message = "Message sent successfully."
        });
    }        

}
