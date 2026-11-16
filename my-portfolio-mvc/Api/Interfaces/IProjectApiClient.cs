using my_portfolio_mvc.Api.Models.Project;
using my_portfolio_mvc.ViewModels.Project;

namespace my_portfolio_mvc.Api.Interfaces;
public interface IProjectApiClient
{
    public Task<List<ProjectDto>> GetAllProjectsAsync();
    public Task<ProjectDto?> GetProjectByIdAsync(long id);
    public Task<List<ProjectDto>> GetFeaturedProjectsAsync();
    public Task<ProjectDto> AddProjectAsync(ProjectViewModel vm, CancellationToken cancellationToken = default);
    public Task<bool> UpdateProjectAsync(ProjectViewModel vm, CancellationToken cancellationToken = default);
    public Task<bool> DeleteProjectAsync(long id, CancellationToken cancellationToken = default);
}
