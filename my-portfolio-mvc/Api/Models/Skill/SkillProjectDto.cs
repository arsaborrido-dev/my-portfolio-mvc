namespace my_portfolio_mvc.Api.Models.Skill;
public sealed class SkillProjectDto
{
    public long Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string ShortDescription { get; init; } = string.Empty;
    public string? ThumbnailImageUrl { get; init; }
}