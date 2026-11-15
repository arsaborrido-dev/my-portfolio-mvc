using my_portfolio_mvc.Api.Interfaces;
using my_portfolio_mvc.Api.Models;
using my_portfolio_mvc.Api.Models.Auth;
using my_portfolio_mvc.Api.Models.Project;
using my_portfolio_mvc.Infrastructure.Http;
using my_portfolio_mvc.ViewModels.Project;
using System.Net;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace my_portfolio_mvc.Api.Clients;
public sealed class ProjectApiClient : IProjectApiClient
{
    private readonly HttpClient _httpClient;
    public ProjectApiClient(HttpClient httpClient) => _httpClient = httpClient;

    //------------------------
    // PROJECTS
    //------------------------
    public async Task<List<ProjectDto>> GetAllProjectsAsync()
    {
        //var allProjects = await _httpClient.GetFromJsonAsync<List<ProjectDto>>("api/projects");
        //return allProjects ?? new List<ProjectDto>();

        var response = await _httpClient.GetAsync("api/projects");

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new ApiUnauthorizedException();
        }

        response.EnsureSuccessStatusCode();

        var allProjectsDto = await response.Content.ReadFromJsonAsync<List<ProjectDto>>();

        return allProjectsDto ?? new List<ProjectDto>();

    }
    public async Task<List<ProjectDto>> GetFeaturedProjectsAsync()
    {

        var response = await _httpClient.GetAsync("api/projects/featured");

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new ApiUnauthorizedException();
        }

        response.EnsureSuccessStatusCode();

        var featProjectsDto = await response.Content.ReadFromJsonAsync<List<ProjectDto>>();

        return featProjectsDto ?? new List<ProjectDto>();

    }

    public async Task<ProjectDto?> GetProjectByIdAsync(long id)
    {
        //var response = await _httpClient.GetFromJsonAsync<ApiResponse<ProjectDto>>(
        //    $"api/projects/{id}");

        var response = await _httpClient.GetAsync(
            $"api/projects/{id}");

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new ApiUnauthorizedException();
        }

        response.EnsureSuccessStatusCode();

        var projDto = await response.Content.ReadFromJsonAsync<ApiResponse<ProjectDto>>();

        return projDto?.Data;

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

    //public async Task UpdateProjectAsync(ProjectViewModel vm, CancellationToken cancellationToken = default)
    //{
    //    var projDto = new ProjectDto
    //    {
    //        Id = vm.Id,
    //        ShortDescription = vm.ShortDescription,
    //        LiveDemoUrl = vm.LiveDemoUrl,
    //        GithubUrl = vm.GithubUrl,
    //        IsFeatured = vm.IsFeatured,
    //        Title = vm.Title
    //    };

    //    var response = await _httpClient.PutAsJsonAsync(
    //        $"api/projects/update",
    //        projDto,
    //        cancellationToken);
    //}

    public async Task<bool> UpdateProjectAsync(
        ProjectViewModel vm,
        CancellationToken cancellationToken = default)
    {
        var request = new ProjectDto
        {
            Id = vm.Id,
            Title = vm.Title,
            ShortDescription = vm.ShortDescription,
            LiveDemoUrl = vm.LiveDemoUrl,
            GithubUrl = vm.GithubUrl,
            IsFeatured = vm.IsFeatured
        };

        using var response = await _httpClient.PutAsJsonAsync(
            $"api/projects/update/{vm.Id}",
            request,
            cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new ApiUnauthorizedException();
        }

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return false;
        }

        response.EnsureSuccessStatusCode();

        return true;
    }


    //public async Task AddProjectAsync(ProjectViewModel vm, CancellationToken cancellationToken = default)
    //{
    //    var projDto = new ProjectDto
    //    {
    //        Id = vm.Id,
    //        ShortDescription = vm.ShortDescription,
    //        LiveDemoUrl = vm.LiveDemoUrl,
    //        GithubUrl = vm.GithubUrl,
    //        IsFeatured = vm.IsFeatured,
    //        Title = vm.Title
    //    };

    //    var response = await _httpClient.PostAsJsonAsync(
    //        $"api/projects/add",
    //        projDto,
    //        cancellationToken);
    //}

    public async Task<ProjectDto> AddProjectAsync(
        ProjectViewModel vm,
        CancellationToken cancellationToken = default)
    {
        var request = new ProjectDto
        {
            Id = vm.Id,
            Title = vm.Title,
            ShortDescription = vm.ShortDescription,
            LiveDemoUrl = vm.LiveDemoUrl,
            GithubUrl = vm.GithubUrl,
            IsFeatured = vm.IsFeatured
        };

        using var response = await _httpClient.PostAsJsonAsync(
            "api/projects/add",
            request,
            cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new ApiUnauthorizedException();
        }

        response.EnsureSuccessStatusCode();

        var createdProject =
            await response.Content.ReadFromJsonAsync<ProjectDto>(
                cancellationToken: cancellationToken);

        return createdProject
            ?? throw new InvalidOperationException(
                "The API created the project but returned an empty response.");
    }

    //softdelete
    //public async Task DeleteProjectAsync(long id, CancellationToken cancellationToken = default)
    //{
    //    var response = _httpClient.PutAsJsonAsync($"api/projects/delete/{id}", id, cancellationToken);
    //}

    // Soft delete
    public async Task<bool> DeleteProjectAsync(
        long id,
        CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.PutAsync(
            $"api/projects/delete/{id}",
            content: null,
            cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new ApiUnauthorizedException();
        }

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return false;
        }

        response.EnsureSuccessStatusCode();

        return true;
    }
}
