using my_portfolio_mvc.ViewModels.Contact;
using my_portfolio_mvc.ViewModels.Project;
using my_portfolio_mvc.ViewModels.Skill;
using my_portfolio_mvc.ViewModels.Technologies;

namespace my_portfolio_mvc.ViewModels.Home;
public class HomeViewModel
{
    public List<ProjectViewModel> FeaturedProjects { get; set; } = new ();
    public List<SkillsListViewModel> Skills { get; set; } = new();
    public List<TechnologiesListViewModel> Technologies { get; set; } = new();
    public ContactMessageViewModel ContactMessage { get; set; } = new();
}
