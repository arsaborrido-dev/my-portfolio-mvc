using my_portfolio_mvc.ViewModels.Project;

namespace my_portfolio_mvc.Services.Interfaces
{
    public interface IProjectService
    {
        public Task<List<ProjectViewModel>> GetAllProjectsAsync();
        public Task<ProjectViewModel?> GetProjectById(long id);
        public Task<List<ProjectViewModel>> GetFeaturedProjects();
    }
}
