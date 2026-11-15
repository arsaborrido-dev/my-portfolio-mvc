using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using my_portfolio_mvc.Services.Interfaces;
using my_portfolio_mvc.ViewModels.Address;
using my_portfolio_mvc.ViewModels.Admin;
using my_portfolio_mvc.ViewModels.Contact;
using my_portfolio_mvc.ViewModels.Home;
using my_portfolio_mvc.ViewModels.Phone;
using my_portfolio_mvc.ViewModels.Project;
using System.Security.Claims;

namespace my_portfolio_mvc.Controllers;

public class AdminController : Controller
{
    private readonly IProjectService _prjSrvc;
    private readonly IAddressService _addSrvc;
    private readonly IPhoneService _phoneSrvc;

    public AdminController(IProjectService prjSrvc, IAddressService addSrvc, IPhoneService phoneSrvc)
    {
        _prjSrvc = prjSrvc;
        _addSrvc = addSrvc;
        _phoneSrvc = phoneSrvc;
    }

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken = default)
    {
        var vm = new AdminViewModel
        {
            Name = string.Empty,
            LastName = string.Empty,
            Projects = await _prjSrvc.GetAllProjectsAsync(),
            Addresses = await _addSrvc.GetAllAddressesAsync(cancellationToken),
            Phones = await _phoneSrvc.GetAllPhonesAsync(cancellationToken)
        };

        return View(vm);
    }

    [HttpGet]
    public async Task<IActionResult> AddPhone(CancellationToken cancellationToken = default)
    {
        var vm = new PhoneViewModel();
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddPhone(PhoneViewModel vm, CancellationToken cancellationToken = default)
    {
        await _phoneSrvc.AddPhoneAsync(vm, cancellationToken);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> AddAddress(CancellationToken cancellationToken = default)
    {
        var vm = new AddressViewModel();
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddAddress(AddressViewModel vm, CancellationToken cancellationToken = default)
    {

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return Forbid();
        }

        vm.UserId = userId;

        await _addSrvc.AddAddressAsync(vm, cancellationToken);
        return RedirectToAction("Index");
    }


    [HttpGet]
    public async Task<IActionResult> EditAddress(long id, CancellationToken cancellationToken = default)
    {    
        var vm = await _addSrvc.GetAddressByIdAsync(id);

        if (vm is null)
        {
            return NotFound();
        }

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditAddress(long id, AddressViewModel vm, CancellationToken cancellationToken = default)
    {
        if (id != vm.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        var updated = await _addSrvc.UpdateAddressAsync(vm, cancellationToken);

        if (!updated)
        {
            return NotFound();
        }

        return RedirectToAction("Index");
    }


    public async Task<IActionResult> DeleteAddress(long id, CancellationToken cancellationToken = default)
    {
        var deleted = await _addSrvc.DeleteAddressAsync(id, cancellationToken);

        if (!deleted)
        {
            return NotFound();
        }

        return RedirectToAction("Index");
    }


    [HttpGet]
    public async Task<IActionResult> AddProject(CancellationToken cancellationToken = default)
    {
        var vm = new ProjectViewModel();
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddProject(ProjectViewModel vm, CancellationToken cancellationToken = default)
    {
        await _prjSrvc.AddProjectAsync(vm);

        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> EditProject(long id, CancellationToken cancellationToken = default)
    {

        var vm = await _prjSrvc.GetProjectById(id);

        if (vm is null)
        {
            return NotFound();
        }

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditProject(long id, ProjectViewModel vm, CancellationToken cancellationToken = default)
    {
        if (id != vm.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {            
            return View(vm);
        }

        var updated = await _prjSrvc.UpdateProjectAsync(vm, cancellationToken);

        if (!updated)
        {
            return NotFound();
        }

        return RedirectToAction("Index");
    }

    //[HttpPut]
    //public async Task<IActionResult> DeletProject(long id, CancellationToken cancellationToken = default)
    //{
    //    await _prjSrvc.DeleteProjectAsync(id, cancellationToken);
    //}
    [HttpPost] // Suitable if this action is triggered by an MVC form
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteProject(
        long id,
        CancellationToken cancellationToken = default)
    {
        var deleted = await _prjSrvc.DeleteProjectAsync(
            id,
            cancellationToken);

        if (!deleted)
        {
            return NotFound();
        }

        return RedirectToAction("Index");
    }

}
