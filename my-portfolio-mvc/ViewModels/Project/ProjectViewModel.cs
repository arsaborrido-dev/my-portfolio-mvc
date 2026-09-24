namespace my_portfolio_mvc.ViewModels.Project;
public class ProjectViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string ShortDescription { get; set; } = "";
    public string LongDescription { get; set; } = "";
    public string? ThumbnailImageUrl { get; set; }        
    public string? GithubUrl { get; set; }
    public string? LiveDemoUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsFeatured { get; set; }
    //public bool IsMainImage { get; set; }
    public List<string> ScreenshotUrls { get; set; } = new();
    public List<string> Skills { get; set; } = new();
    public List<string> Technologies { get; set; } = new();

}
