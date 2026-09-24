using my_portfolio_mvc.ViewModels.Skill;

namespace my_portfolio_mvc.Services.Interfaces
{
    public interface ISkillService
    {
        public Task<List<SkillViewModel>> GetAllSkillsAsync();
        public Task<SkillViewModel?> GetSkillById(int id);
    }
}
