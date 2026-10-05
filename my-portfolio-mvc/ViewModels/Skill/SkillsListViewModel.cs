

namespace my_portfolio_mvc.ViewModels.Skill;

public sealed class SkillsListViewModel
{
    public long Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string SkillCategory { get; init; } = string.Empty;
}
