
using my_portfolio_mvc.Api.Interfaces;
using my_portfolio_mvc.Api.Models.Project;
using my_portfolio_mvc.Services.Interfaces;
using my_portfolio_mvc.ViewModels.Project;

namespace my_portfolio_mvc.Services.Implementations;
public class ProjectService : IProjectService
{
    private readonly IProjectApiClient _prjApi;
    public ProjectService(IProjectApiClient prjApi) => _prjApi = prjApi;
    public async Task<List<ProjectViewModel>> GetAllProjectsAsync()
    {
        var projects = await _prjApi.GetAllProjectsAsync();
        return projects.Select(MapToVm).ToList();
    }
    public async Task<ProjectViewModel?> GetProjectById(long id)
    {
        var project = await _prjApi.GetProjectByIdAsync(id);

        return (project is null
                 ? null
                 : MapToVm(project));

        //return project;
    }
    public async Task<List<ProjectViewModel>> GetFeaturedProjects()
    {
        var featuredProjects = await _prjApi.GetFeaturedProjectsAsync();
        return featuredProjects.Select(MapToVm).ToList(); ;
    }
    private static ProjectViewModel MapToVm(ProjectDto dto)
    {
        return new ProjectViewModel
        {
            Id = dto.Id,
            Title= dto.Title,
            ShortDescription = dto.ShortDescription,
            LongDescription = dto.LongDescription,
            ProjectDemonstration = dto.ProjectDemonstration,
            ThumbnailImageUrl = dto.ThumbnailImageUrl,
            GithubUrl = dto.GithubUrl,
            LiveDemoUrl = dto.LiveDemoUrl,
            CreatedAtUtc = dto.CreatedAtUtc,
            IsFeatured = dto.IsFeatured,
            Screenshots = dto.Screenshots,
            Skills = dto.Skills,
            Technologies = dto.Technologies
        };
    }
}
