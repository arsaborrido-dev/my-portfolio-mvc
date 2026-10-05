using my_portfolio_mvc.Api.Interfaces;
using my_portfolio_mvc.Api.Models.Skill;
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
    public async Task<List<SkillsListDto>> GetAllSkillsAsync()
    {
        var allSkills = await _httpClient.GetFromJsonAsync<List<SkillsListDto>>(
            "api/skills"
        );

        return allSkills ?? new List<SkillsListDto>();
    }
    public async Task<SkillDto?> GetSkillByIdAsync(long id)
    {
        var skill = await _httpClient.GetFromJsonAsync<SkillDto>($"api/skills/{id}");
        return skill;
    }
}
