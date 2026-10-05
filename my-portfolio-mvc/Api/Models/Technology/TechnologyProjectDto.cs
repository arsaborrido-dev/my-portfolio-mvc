namespace my_portfolio_mvc.Api.Models.Technology;

public sealed class TechnologyProjectDto
{
    public long Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string ShortDescription { get; init; } = string.Empty;
    public string? ThumbnailImageUrl { get; init; }
}
