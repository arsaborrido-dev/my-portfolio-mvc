using my_portfolio_mvc.Api.Models;
using my_portfolio_mvc.ViewModels.Skill;

namespace my_portfolio_mvc.Api.Interfaces;
public interface ISkillApiService
{
    public Task<List<SkillDto>> GetAllSkillsAsync();
    public Task<SkillDto?> GetSkillByIdAsync(int id);
}
