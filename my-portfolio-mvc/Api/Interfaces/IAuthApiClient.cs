using my_portfolio_mvc.Api.Models.Auth;
using my_portfolio_mvc.ViewModels.Home;

namespace my_portfolio_mvc.Api.Interfaces;
public interface IAuthApiClient
{
    Task<RefreshResult> RefreshAsync(CancellationToken cancellationToken);
    Task<LoginResult> LoginAsync(LoginViewModel vm, CancellationToken cancellationToken);
    Task<RegisterResult> RegisterAsync(UserRegisterViewModel vm, CancellationToken cancellationToken);
    Task LogoutAsync(CancellationToken cancellationToken = default);
}