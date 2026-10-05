namespace my_portfolio_mvc.Api.Models.Project;
public sealed class ProjectSkillDto
{
    public long Id { get; init; }
    public string Name { get; set; } = string.Empty;
    public string SkillCategory { get; set; } = string.Empty;
}
