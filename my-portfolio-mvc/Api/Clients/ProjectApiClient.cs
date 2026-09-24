using my_portfolio_mvc.Api.Interfaces;
using my_portfolio_mvc.ViewModels.Project;
using my_portfolio_mvc.Api.Models;

namespace my_portfolio_mvc.Api.Clients;
public class ProjectApiClient : IProjectApiClient
{
    private readonly HttpClient _httpClient;
    public ProjectApiClient(HttpClient httpClient) => _httpClient = httpClient;

    //------------------------
    // PROJECTS
    //------------------------
    public async Task<List<ProjectDto>> GetAllProjectsAsync()
    {
        var allProjects = await _httpClient.GetFromJsonAsync<List<ProjectDto>>("api/projects");
        return allProjects ?? new List<ProjectDto>();
    }
    public async Task<List<ProjectDto>> GetFeaturedProjectsAsync()
    {
        var featuredProjects = await _httpClient.GetFromJsonAsync<List<ProjectDto>>("api/projects/featured");
        return featuredProjects ?? new List<ProjectDto>();
    }
    public async Task<ProjectDto?> GetProjectByIdAsync(int id)
    {
        var project = await _httpClient.GetFromJsonAsync<ProjectDto>($"api/projects/{id}");
        return project;
    }

}
