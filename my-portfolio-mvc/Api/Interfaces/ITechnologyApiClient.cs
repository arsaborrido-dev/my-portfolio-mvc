using my_portfolio_mvc.Api.Models.Technology;

namespace my_portfolio_mvc.Api.Interfaces;
public interface ITechnologyApiClient
{
    public Task<List<TechnologiesListDto>> GetAllTechnologiesAsync();
    public Task<TechnologyDto?> GetTechnologyByIdAsync(long id);
}
