// This behavior is executed before the actual request handler is invoked
// and is responsible for logging information about the incoming request.
// When you dispatch any Command,
// the LoggingBehaviour will log information about the request before the CreateUserCommandHandler executes its logic.
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace Elearninig.Base.Application.Behaviors.MediatR;

public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    private readonly ILogger<TRequest> _logger;

    public LoggingBehaviour(ILogger<TRequest> logger)
    {
        _logger = logger;
    }
    // The Process method is called by MediatR before the actual request handler is executed
    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        _logger.LogInformation($"Request : {requestName}");
    }
}
