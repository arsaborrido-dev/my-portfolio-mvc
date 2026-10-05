namespace my_portfolio_mvc.Api.Models.Project;
public sealed class ProjectScreenshotDto
{
    public long Id { get; init; }
    public string ImageUrl { get; set; } = string.Empty;
}
