using my_portfolio_mvc.Api.Interfaces;
using my_portfolio_mvc.Api.Models.Technology;
using my_portfolio_mvc.Services.Interfaces;
using my_portfolio_mvc.ViewModels.Technologies;

namespace my_portfolio_mvc.Services.Implementations;

public class TechnologyService : ITechnologyService
{
    public readonly ITechnologyApiClient _technologyApiClient;

    public TechnologyService(ITechnologyApiClient technologyApiClient) => _technologyApiClient = technologyApiClient;
    public async Task<List<TechnologiesListViewModel>> GetAllTechnologies()
    {
        var allTechnologies = await _technologyApiClient.GetAllTechnologiesAsync();
        return allTechnologies.Select(MaptoListVm).ToList();
    }

    public async Task<TechnologyViewModel?> GetTechnologyById(long id)
    {
        var technology = await _technologyApiClient.GetTechnologyByIdAsync(id);
        return technology is null
            ? null
            : MaptoVm(technology);
    }

    private static TechnologyViewModel MaptoVm(TechnologyDto dto)
    {
        return new TechnologyViewModel
        {
            Id = dto.Id,
            Name = dto.Name,
            Projects = dto.Projects
        };
    }

    private static TechnologiesListViewModel MaptoListVm(TechnologiesListDto dto)
    {
        return new TechnologiesListViewModel
        {
            Id = dto.Id,
            Name = dto.Name            
        };
    }
}
