using my_portfolio_mvc.Api.Interfaces;
using my_portfolio_mvc.Api.Models.Skill;
using my_portfolio_mvc.Services.Interfaces;
using my_portfolio_mvc.ViewModels.Project;
using my_portfolio_mvc.ViewModels.Skill;

namespace my_portfolio_mvc.Services.Implementations;
public class SkillService : ISkillService
{
    private readonly ISkillApiService _skillApi;
    public SkillService(ISkillApiService skillApi) => _skillApi = skillApi;

    public async Task<List<SkillsListViewModel>> GetAllSkillsAsync()
    {
        var allSkills = await _skillApi.GetAllSkillsAsync();
        return allSkills.Select(MapToListVm).ToList();
    }

    public async Task<SkillViewModel?> GetSkillById(long id)
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
    private static SkillsListViewModel MapToListVm(SkillsListDto dto)
    {
        return new SkillsListViewModel
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description,
            SkillCategory = dto.SkillCategory
        };
    }
}
