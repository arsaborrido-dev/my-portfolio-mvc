using Microsoft.AspNetCore.Mvc;
using my_portfolio_mvc.Security;
using my_portfolio_mvc.Services.Interfaces;
using my_portfolio_mvc.ViewModels;
using my_portfolio_mvc.ViewModels.Project;

namespace my_portfolio_mvc.Controllers;
public class ProjectController : Controller
{
    private readonly IProjectService _prjSrvc;
    public ProjectController(IProjectService prjSrvc)
        => _prjSrvc = prjSrvc;

    //public async Task<IActionResult> Index()
    //{
    //    var projects = await _projectService.GetProjectsAsync();
    //    return View(projects); // passes List<ProjectsViewModel> to View
    //}

    public async Task<IActionResult> ProjectDetail(long id)
    {
        var project = await _prjSrvc.GetProjectById(id);
        if (project == null)
            return NotFound();

        project.ShortDescription = HtmlSanitizerHelper.Sanitize(project.ShortDescription);
        project.LongDescription = HtmlSanitizerHelper.Sanitize(project.LongDescription);
        project.ProjectDemonstration = HtmlSanitizerHelper.Sanitize(project.ProjectDemonstration);

        return View(project);
    }
    public async Task<IActionResult> ProjectsList(string search, int page = 1)
    {
        ViewData["CurrentSearch"] = search;

        var allProjects = await _prjSrvc.GetAllProjectsAsync();

        // SEARCH
        if (!string.IsNullOrWhiteSpace(search))
            allProjects = allProjects.Where(e => e.Title.ToLower().Contains(search.ToLower())).ToList();

        // PAGINATION
        int pageSize = 5;
        var count = allProjects.Count();
        var pagedProjects = allProjects
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var vm = new ProjectsListViewModel
        {
            ProjectViewModels = pagedProjects,
            Search = search,
            //SortOrder = sortOrder,
            PageNumber = page,
            TotalPages = (int)Math.Ceiling(count / (double)pageSize)
        };

        return View(vm);

    }
}
