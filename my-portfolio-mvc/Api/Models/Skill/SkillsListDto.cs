namespace my_portfolio_mvc.Api.Models.Skill;

public sealed class SkillsListDto
{
    public long Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string SkillCategory { get; init; } = string.Empty;
}
