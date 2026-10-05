using my_portfolio_mvc.Api.Models.Auth;

namespace my_portfolio_mvc.Api.Interfaces;
public interface IAuthApiClient
{
    Task<RefreshResult> RefreshAsync(CancellationToken cancellationToken);
}
