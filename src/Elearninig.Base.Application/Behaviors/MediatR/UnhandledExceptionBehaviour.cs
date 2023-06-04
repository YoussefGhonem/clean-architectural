using MediatR;
using Microsoft.Extensions.Logging;

// this fire when ocures a Exception inside handlers

//  pipeline behavior in MediatR. This behavior intercepts the processing of requests and
//  handles any unhandled exceptions that occur during the execution of request handlers.
namespace Elearninig.Base.Application.Behaviors.MediatR;

//IPipelineBehavior interface from MediatR, which allows it to intercept the processing of requests.
public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly ILogger<TRequest> _logger;

    public UnhandledExceptionBehaviour(ILogger<TRequest> logger)
    {
        _logger = logger;
    }
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            var requestName = typeof(TRequest).Name;

            _logger.LogError(ex, "CleanArchitecture Request: Unhandled Exception for Request {Name} {@Request}", requestName, request);

            throw;
        }
    }
}
