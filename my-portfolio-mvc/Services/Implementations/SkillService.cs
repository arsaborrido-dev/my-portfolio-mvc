using my_portfolio_mvc.Api.Interfaces;
using my_portfolio_mvc.Api.Models;
using my_portfolio_mvc.Services.Interfaces;
using my_portfolio_mvc.ViewModels.Project;
using my_portfolio_mvc.ViewModels.Skill;

namespace my_portfolio_mvc.Services.Implementations;
public class SkillService : ISkillService
{
    private readonly ISkillApiService _skillApi;
    public SkillService(ISkillApiService skillApi) => _skillApi = skillApi;

    public async Task<List<SkillViewModel>> GetAllSkillsAsync()
    {
        var allSkills = await _skillApi.GetAllSkillsAsync();
        return allSkills.Select(MapToVm).ToList();
    }

    public async Task<SkillViewModel?> GetSkillById(int id)
    {
        var skill = await _skillApi.GetSkillByIdAsync(id);            

        return (skill is null
           ? null
           : MapToVm(skill));

        //return skill;
    }

    private static SkillViewModel MapToVm(SkillDto dto)
    {
        return new SkillViewModel
        {
            Id = dto.Id,
            Name = dto.Name,
            Projects = dto.Projects
        };
    }

}
