using my_portfolio_mvc.Api.Interfaces;
using my_portfolio_mvc.Api.Models;
using my_portfolio_mvc.Api.Models.Project;
using my_portfolio_mvc.ViewModels.Project;
using System.Text.Json;

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
    public async Task<ProjectDto?> GetProjectByIdAsync(long id)
    {
        var response = await _httpClient.GetFromJsonAsync<ApiResponse<ProjectDto>>(
            $"api/projects/{id}");

        return response?.Data;

        //Console.WriteLine($"PROJECT SEARCH: {id}");
        //var project = await _httpClient.GetFromJsonAsync<ProjectDto>($"api/projects/{id}");

        //Console.WriteLine($"Project exists: {!string.IsNullOrWhiteSpace(project.Title)}");

        //return project;
        //var response = await _httpClient.GetAsync($"api/projects/{id}");

        //var content = await response.Content.ReadAsStringAsync();

        //Console.WriteLine($"API Status: {response.StatusCode}");
        //Console.WriteLine($"API Response: {content}");

        //response.EnsureSuccessStatusCode();

        //return JsonSerializer.Deserialize<ProjectDto>(
        //    content,
        //    new JsonSerializerOptions
        //    {
        //        PropertyNameCaseInsensitive = true
        //    });
    }

}
