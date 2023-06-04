using Elearninig.Base.Application.common.RedisCache.Extensions;
using Elearninig.Base.Application.common.RedisCache.models;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ServiceStack.Redis;
using StackExchange.Redis;

namespace Elearninig.Base.Application.common.RedisCache.Services;

// docker run -p6379:6379 redis. you should run that command to initiate docker image for redis.
// also you can download redis desktop manager to watch how the cache is persisted in the memory.
public class ResponseCacheService : IResponseCacheService
{
    // redis cache
    private readonly IDistributedCache _distributedCache;
    private readonly RedisCachingOptions _config;


    public ResponseCacheService(IDistributedCache distributedCache, IConfiguration configuration)
    {
        _config = configuration.GetRedisCasheConfig();
        _distributedCache = distributedCache;
    }

    public async Task CacheResponseAsync(string cacheKey, object? response, TimeSpan timeToLive,
        CancellationToken cancellationToken)
    {
        if (response == null)
        {
            return;
        }

        var serializedResponse = JsonConvert.SerializeObject(response);
        await _distributedCache.SetStringAsync(cacheKey, serializedResponse,
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = timeToLive
            }, cancellationToken);
    }

    // get the cache response

    public async Task<string> GetCachedResponseAsync(string cacheKey, CancellationToken cancellationToken)
    {
        var cachedResponse = await _distributedCache.GetStringAsync(cacheKey, cancellationToken);
        return (string.IsNullOrEmpty(cachedResponse) ? null : cachedResponse) ?? string.Empty;
    }

    // refresh the cache response on adding some changes such as add, edit or delete

    public async Task RefreshCacheResponseAsync(string cacheKey, CancellationToken cancellationToken)
    {
        await _distributedCache.RefreshAsync(cacheKey, cancellationToken);
    }


    public async Task ResetCacheResponseAsync(string? cacheKeyPrefix, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(cacheKeyPrefix)
            || !_config.Enabled
            || string.IsNullOrWhiteSpace(_config.ConnectionString))
        {
            return;
        }

        // get all keys that may contain cache key
        using var redis = await ConnectionMultiplexer.ConnectAsync(_config.ConnectionString);
        IAsyncEnumerable<RedisKey> keys = redis.GetServer(GetHostName(_config.ConnectionString),
            GetHostPort(_config.ConnectionString)).KeysAsync(0, cacheKeyPrefix + "*", 1000);
        await foreach (var key in keys.WithCancellation(cancellationToken))
        {
            await _distributedCache.RemoveAsync(key, cancellationToken);
        }
    }

    private static string GetHostName(string connectionString)
        => connectionString.Split(":").First();

    private static int GetHostPort(string connectionString)
        => connectionString.Split(":").Length > 1
           && connectionString.Split(":")[1].All(char.IsDigit)
            ? int.Parse(connectionString.Split(":")[1])
            : 6379;
}