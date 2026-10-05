namespace my_portfolio_mvc.Api.Models.Skill;
public sealed class SkillDto
{
    //public long Id { get; set; }
    //public string Name { get; set; } = string.Empty;
    //public List<string> Projects { get; set; } = new();

    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string SkillCategory { get; set; } = string.Empty;
    public List<SkillProjectDto> Projects { get; init; } = [];


}
