using Elearninig.Base.Application.common.RedisCache.models;
using Microsoft.Extensions.Configuration;

namespace Elearninig.Base.Application.common.RedisCache.Extensions;
public static class RedisCachingOptionsExtension
{
    public static RedisCachingOptions GetRedisCasheConfig(this IConfiguration configuration)
    {
        var config = configuration.GetSection("RedisCache").Get<RedisCachingOptions>();

        if (config is null)
        {
            throw new Exception("Missing 'RedisCache' configuration section from the appsettings.");
        }

        return config;
    }
}
