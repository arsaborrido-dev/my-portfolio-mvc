using my_portfolio_mvc.Api.Interfaces;
using my_portfolio_mvc.Api.Models;
using my_portfolio_mvc.ViewModels.Skill;
using static System.Net.WebRequestMethods;

namespace my_portfolio_mvc.Api.Clients;
public class SkillApiClient : ISkillApiService
{
    private readonly HttpClient _httpClient;

    public SkillApiClient(HttpClient httpClient) => _httpClient = httpClient;

    //------------------------
    // SKILLS
    //------------------------
    public async Task<List<SkillDto>> GetAllSkillsAsync()
    {
        var allSkills = await _httpClient.GetFromJsonAsync<List<SkillDto>>(
            "api/skills"
        );

        return allSkills ?? new List<SkillDto>();
    }
    public async Task<SkillDto?> GetSkillByIdAsync(int id)
    {
        var skill = await _httpClient.GetFromJsonAsync<SkillDto>($"api/skills/{id}");
        return skill;
    }
}
