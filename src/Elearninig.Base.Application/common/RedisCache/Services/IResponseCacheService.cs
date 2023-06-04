namespace Elearninig.Base.Application.common.RedisCache.Services;

public interface IResponseCacheService
{

    Task CacheResponseAsync(string cacheKey, object? response, TimeSpan timeToLive,
        CancellationToken cancellationToken);
    Task<string> GetCachedResponseAsync(string cacheKey, CancellationToken cancellationToken);
    Task RefreshCacheResponseAsync(string cacheKey, CancellationToken cancellationToken);
    Task ResetCacheResponseAsync(string? cacheKey, CancellationToken cancellationToken);
}