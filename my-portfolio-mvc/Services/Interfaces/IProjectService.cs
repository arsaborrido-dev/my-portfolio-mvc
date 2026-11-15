using my_portfolio_mvc.Api.Models.Project;
using my_portfolio_mvc.ViewModels.Project;

namespace my_portfolio_mvc.Services.Interfaces;
public interface IProjectService
{
    public Task<List<ProjectViewModel>> GetAllProjectsAsync();
    public Task<ProjectViewModel?> GetProjectById(long id);
    public Task<List<ProjectViewModel>> GetFeaturedProjects();
    public Task<bool> UpdateProjectAsync(ProjectViewModel vm, CancellationToken cancellationToken = default);
    public Task<ProjectDto> AddProjectAsync(ProjectViewModel vm, CancellationToken cancellationToken = default);
    public Task<bool> DeleteProjectAsync(long id, CancellationToken cancellationToken = default);


}
