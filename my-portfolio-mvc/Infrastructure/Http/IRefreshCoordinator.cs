
using my_portfolio_mvc.Api.Models.Auth;

namespace my_portfolio_mvc.Infrastructure.Http;

public interface IRefreshCoordinator
{
    Task<RefreshResult> RefreshAsync(
        string refreshToken,
        Func<CancellationToken, Task<RefreshResult>> refreshOperation,
        CancellationToken cancellationToken);
}
