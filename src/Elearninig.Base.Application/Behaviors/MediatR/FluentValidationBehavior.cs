using Elearninig.Base.Application.GlobalExceptions;
using FluentValidation;
using MediatR;

namespace Elearninig.Base.Application.Behaviors.MediatR;
// this a pipeline behavior in MediatR that is responsible for validating the request before it reaches the corresponding request handler.
// It intercepts the processing of requests and performs validation using a collection of validators.
public class FluentValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators; // These validators are responsible for validating any request.

    public FluentValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            // object using the request that is being processed. The ValidationContext provides a container for performing validation.
            var validationContext = new ValidationContext<TRequest>(request);

            var validatorsResult = await Task.WhenAll(
                _validators.Select(x => x.ValidateAsync(validationContext, cancellationToken)));

            if (validatorsResult.Any())
            {
                var failures = validatorsResult.Where(x => x.Errors.Any()).SelectMany(c => c.Errors).ToList();

                if (failures.Any())
                    throw new CustomValidationException(failures);
            }
        }
        return await next();

    }
}
