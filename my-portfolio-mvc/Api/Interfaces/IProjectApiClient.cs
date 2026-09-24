using my_portfolio_mvc.Api.Models;
using my_portfolio_mvc.ViewModels.Project;

namespace my_portfolio_mvc.Api.Interfaces;
public interface IProjectApiClient
{
    public Task<List<ProjectDto>> GetAllProjectsAsync();
    public Task<ProjectDto?> GetProjectByIdAsync(int id);
    public Task<List<ProjectDto>> GetFeaturedProjectsAsync();
}
