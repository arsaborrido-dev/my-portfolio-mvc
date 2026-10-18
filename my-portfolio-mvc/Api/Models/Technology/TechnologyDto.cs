using my_portfolio_mvc.Api.Models.Skill;

namespace my_portfolio_mvc.Api.Models.Technology;

public sealed class TechnologyDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string SkillCategory { get; set; } = string.Empty;
    public List<SkillProjectDto> Projects { get; init; } = [];

}
