using my_portfolio_mvc.Api.Interfaces;
using my_portfolio_mvc.Api.Models.Technology;

namespace my_portfolio_mvc.Api.Clients;

public class TechnologyApiClient : ITechnologyApiClient
{
    private readonly HttpClient _httpClient;
    public TechnologyApiClient(HttpClient httpClient) => _httpClient = httpClient;  

    public async Task<List<TechnologiesListDto>> GetAllTechnologiesAsync()
    {
        var allTechnologies = await _httpClient.GetFromJsonAsync<List<TechnologiesListDto>>("api/technologies");

        return allTechnologies ?? new List<TechnologiesListDto>();
    }

    public async Task<TechnologyDto?> GetTechnologyByIdAsync(long id)
    {
        var technology = await _httpClient.GetFromJsonAsync<TechnologyDto>($"api/technologies/{id}");
        return technology;
    }
}
