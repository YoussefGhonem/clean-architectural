using Elearninig.Base.Application.common.RedisCache.Constants;
using Elearninig.Base.Application.common.RedisCache.Extensions;
using Elearninig.Base.Application.common.RedisCache.Interfaces;
using Elearninig.Base.Application.common.RedisCache.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Elearninig.Base.Application.Behaviors.MediatR.Caching;

// This behavior is used to introduce caching functionality into the request pipeline.
public class RedisCachingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;

    public RedisCachingBehaviour(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
    {
        //  These dependencies are used to access the current HttpContext and retrieve configuration settings,
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)

    {
        if (!_configuration.GetRedisCasheConfig().Enabled)
            return await next();

        // retrieves an instance of IResponseCacheService from the HttpContext to interact with the caching mechanism.
        // This service is responsible for storing and retrieving cached responses.
        var cacheService =
            _httpContextAccessor.HttpContext?.RequestServices.GetRequiredService<IResponseCacheService>();

        var requestType = request.GetType();
        // The method checks if the request ( Query ) implements / inherent the 'ICacheableQuery' interface ( to use cashing in your handler)
        if (!typeof(ICacheableQuery).IsAssignableFrom(requestType)) return await next();

        // The GetKey method is used to generate a unique key for the cached response based on the request type
        // and its properties. It serializes the request's properties into a JSON string and includes the request type's full name in the key.
        var key = GenerateKey(request);

        var cachedResponse = await cacheService!.GetCachedResponseAsync(key, cancellationToken);
        // It checks if a cached response exists for the generated key. If a cached response is found,
        // it deserializes it back into the expected response type and
        // returns it immediately, bypassing the execution of the next behavior.
        if (!string.IsNullOrEmpty(cachedResponse))
            return JsonConvert.DeserializeObject<TResponse>(cachedResponse)!;

        var response = await next();

        // The method then caches the response by calling the CacheResponseAsync method of the IResponseCacheService instance.
        // It specifies the key, the response object serialized as a JSON string, and a time span indicating the cache expiration time.
        await cacheService.CacheResponseAsync(key, response,
            TimeSpan.FromSeconds(CacheSpan.Day), cancellationToken);

        return response;
    }

    private string GenerateKey(TRequest request)
    {
        var properties = JsonConvert.SerializeObject(request.GetType().GetProperties()
            .Select(p => $"{p.Name}:{GetValObjDy(request, p.Name)}"));
        return $"{request.GetType().FullName}: {properties}";
    }

    private object? GetValObjDy(object obj, string propertyName)
    {
        return obj.GetType().GetProperty(propertyName)?.GetValue(obj, null);
    }
}