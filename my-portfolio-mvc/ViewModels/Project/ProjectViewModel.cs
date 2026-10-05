using my_portfolio_mvc.Api.Models.Project;

namespace my_portfolio_mvc.ViewModels.Project;
public class ProjectViewModel
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public string LongDescription { get; set; } = string.Empty;
    public string ProjectDemonstration { get; set; } = string.Empty;
    public string? ThumbnailImageUrl { get; set; }        
    public string? GithubUrl { get; set; }
    public string? LiveDemoUrl { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
    public bool IsFeatured { get; set; }
    public List<ProjectScreenshotDto> Screenshots { get; init; } = [];
    public List<ProjectSkillDto> Skills { get; init; } = [];
    public List<ProjectTechnologyDto> Technologies { get; init; } = [];
    //public bool IsMainImage { get; set; }
    //public List<string> ScreenshotUrls { get; set; } = new();
    //public List<string> Skills { get; set; } = new();
    //public List<string> Technologies { get; set; } = new();

}
