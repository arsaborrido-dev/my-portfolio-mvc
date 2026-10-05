using my_portfolio_mvc.ViewModels.Technologies;

namespace my_portfolio_mvc.Services.Interfaces
{
    public interface ITechnologyService
    {
        Task<List<TechnologiesListViewModel>> GetAllTechnologies();
        Task<TechnologyViewModel?> GetTechnologyById(long id);
    }
}
