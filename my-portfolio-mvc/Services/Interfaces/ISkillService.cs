using my_portfolio_mvc.ViewModels.Skill;

namespace my_portfolio_mvc.Services.Interfaces;
public interface ISkillService
{
    public Task<List<SkillsListViewModel>> GetAllSkillsAsync();
    public Task<SkillViewModel?> GetSkillById(long id);
}
