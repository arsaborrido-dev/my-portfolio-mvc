
namespace my_portfolio_mvc.Api.Models.Auth;

public enum RefreshFailureReason
{
    None,
    Rejected,
    Unavailable
}

public sealed class RefreshResult
{
    public bool Success { get; set; }

    public RefreshFailureReason FailureReason { get; set; }
        = RefreshFailureReason.None;

    public string AccessToken { get; set; } = string.Empty;

    public DateTimeOffset AccessTokenExpiresAtUtc { get; set; }

    public string RefreshToken { get; set; } = string.Empty;

    public DateTimeOffset RefreshTokenExpiresAtUtc { get; set; }
}