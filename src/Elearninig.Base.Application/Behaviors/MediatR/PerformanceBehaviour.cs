using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Elearninig.Base.Application.Behaviors.MediatR;
//  It is used as a behavior in the MediatR pipeline to measure the performance of request handling and log warnings for long-running requests.
public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly Stopwatch _stopwatch;
    private readonly ILogger _logger;

    public PerformanceBehaviour(Stopwatch stopwatch, ILogger logger)
    {
        _stopwatch = stopwatch;
        _logger = logger;
    }
    // method is the main logic of the behavior. It measures the elapsed time for request handling,
    // logs a warning if the elapsed time exceeds 500 milliseconds, and returns the response.
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _stopwatch.Start();
        var response = await next();
        _stopwatch.Stop();
        var timer = _stopwatch.ElapsedMilliseconds;
        if (timer > 500)
        {
            var requestName = typeof(TRequest).Name;

            _logger.LogWarning("Long Running Request: {Name} ({timer} milliseconds) {@Request}",
                requestName, timer, request);
        }
        return response;
    }
}

