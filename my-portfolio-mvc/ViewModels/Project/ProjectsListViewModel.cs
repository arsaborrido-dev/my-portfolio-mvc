namespace my_portfolio_mvc.ViewModels.Project;
public class ProjectsListViewModel
{
    public IEnumerable<ProjectViewModel> ProjectViewModels { get; set; } = new List<ProjectViewModel>();
    public string Search { get; set; } = "";
    public string SortOrder { get; set; } = "";
    public int PageNumber { get; set; }
    public int TotalPages { get; set; }

}
