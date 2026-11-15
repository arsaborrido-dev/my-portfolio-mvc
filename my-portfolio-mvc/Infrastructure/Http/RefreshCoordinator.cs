
using my_portfolio_mvc.Api.Models.Auth;
using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;

namespace my_portfolio_mvc.Infrastructure.Http;

public sealed class RefreshCoordinator : IRefreshCoordinator
{
    private static readonly TimeSpan CacheLifetime =
        TimeSpan.FromSeconds(30);

    private readonly SemaphoreSlim _refreshLock = new(1, 1);

    private readonly ConcurrentDictionary<string, CacheEntry> _cache =
        new();

    public async Task<RefreshResult> RefreshAsync(
        string refreshToken,
        Func<CancellationToken, Task<RefreshResult>> refreshOperation,
        CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(refreshToken);
        ArgumentNullException.ThrowIfNull(refreshOperation);

        var tokenKey = CreateTokenKey(refreshToken);

        await _refreshLock.WaitAsync(cancellationToken);

        try
        {
            RemoveExpiredEntries();

            // Another request may already have refreshed this token.
            if (_cache.TryGetValue(tokenKey, out var cachedEntry))
            {
                return Clone(cachedEntry.Result);
            }

            // This request performs the actual refresh.
            var result = await refreshOperation(cancellationToken);

            // Cache only successful, complete token rotations.
            if (result.Success &&
                !string.IsNullOrWhiteSpace(result.AccessToken) &&
                !string.IsNullOrWhiteSpace(result.RefreshToken))
            {
                _cache[tokenKey] = new CacheEntry(
                    Clone(result),
                    DateTimeOffset.UtcNow.Add(CacheLifetime));
            }

            return Clone(result);
        }
        finally
        {
            _refreshLock.Release();
        }
    }

    private void RemoveExpiredEntries()
    {
        var now = DateTimeOffset.UtcNow;

        foreach (var entry in _cache)
        {
            if (entry.Value.ExpiresAtUtc <= now)
            {
                _cache.TryRemove(entry.Key, out _);
            }
        }
    }

    private static string CreateTokenKey(string refreshToken)
    {
        var bytes = Encoding.UTF8.GetBytes(refreshToken);
        var hash = SHA256.HashData(bytes);

        return Convert.ToHexString(hash);
    }

    private static RefreshResult Clone(RefreshResult result)
    {
        return new RefreshResult
        {
            Success = result.Success,
            FailureReason = result.FailureReason,
            AccessToken = result.AccessToken,
            AccessTokenExpiresAtUtc = result.AccessTokenExpiresAtUtc,
            RefreshToken = result.RefreshToken,
            RefreshTokenExpiresAtUtc = result.RefreshTokenExpiresAtUtc
        };
    }

    private sealed record CacheEntry(
        RefreshResult Result,
        DateTimeOffset ExpiresAtUtc);
}
