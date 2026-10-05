using my_portfolio_mvc.Api.Models.Skill;
using my_portfolio_mvc.ViewModels.Skill;

namespace my_portfolio_mvc.Api.Interfaces;
public interface ISkillApiService
{
    public Task<List<SkillsListDto>> GetAllSkillsAsync();
    public Task<SkillDto?> GetSkillByIdAsync(long id);
}
