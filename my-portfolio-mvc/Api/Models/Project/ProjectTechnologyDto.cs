namespace my_portfolio_mvc.Api.Models.Project;
public class ProjectTechnologyDto
{
    public long Id { get; init; }
    public string Name { get; set; } = string.Empty;
    public string TechnologyCategory { get; set; } = string.Empty;
}
