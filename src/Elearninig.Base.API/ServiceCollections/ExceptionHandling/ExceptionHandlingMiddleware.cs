using Elearninig.Base.Application.GlobalExceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Elearninig.Base.API.ServiceCollections.ExceptionHandling;


public sealed class ExceptionHandlingMiddleware : IMiddleware
{

	private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
		try
		{
			await next(context);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex.Message);
            await HandleExceptionAsync(context, ex);
		}
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
		int statusCode;
		object response;

		switch (exception)
		{
			case UnauthorizedAccessException:
				statusCode = StatusCodes.Status401Unauthorized;
                response = new
				{
					title = "Unauthorized",
					status = statusCode,
					detail = exception.Message
				};
				break;	
			case BadRequestException:
				statusCode = StatusCodes.Status400BadRequest;
                response = new
				{
					title = "BadRequest",
					status = statusCode,
					detail = exception.Message
				};
				break;		
			case ForbiddenException:
				statusCode = StatusCodes.Status403Forbidden;
                response = new
				{
					title = "Forbidden",
					status = statusCode,
					detail = exception.Message
				};
				break;	
			case Application.GlobalExceptions.KeyNotFoundException:
				statusCode = StatusCodes.Status404NotFound;
                response = new
				{
					title = "Key Not Found",
					status = statusCode,
					detail = exception.Message
				};
				break;
            case Application.GlobalExceptions.NotImplementedException:
                statusCode = StatusCodes.Status501NotImplemented;
                response = new
                {
                    title = "Not Implemented",
                    status = statusCode,
                    detail = exception.Message
                };
                break;
            case NotFoundException:
				statusCode = StatusCodes.Status404NotFound;
                response = new
				{
					title = "Not Found",
					status = statusCode,
					detail = exception.Message
				};
				break; 
			case CustomValidationException:
                statusCode = StatusCodes.Status400BadRequest;
                response = new
                {
                    title = "Validation Errors",
                    status = statusCode,
                    detail = exception.Message,
                    errors = GetErrors(exception)
                };
                break;
            default:
                statusCode = StatusCodes.Status500InternalServerError;
                response = new
                {
                    title = "Internal server error",
                    status = statusCode,
                    detail = exception.Message,
                };
                break;

        }

		context.Response.StatusCode = statusCode;
		context.Response.ContentType= "application/json";

		await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
    private static IReadOnlyDictionary<string, string[]> GetErrors(Exception exception)
    {
        IReadOnlyDictionary<string, string[]> errors = new Dictionary<string, string[]>();

        if (exception is CustomValidationException validationException)
        {
            errors = validationException.ErrorsDictionary;
        }

        return errors;
    }
}