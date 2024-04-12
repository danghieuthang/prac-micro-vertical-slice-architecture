using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using TodoApp.Application.Core.Exceptions;
using TodoApp.Infrastructure.Core.Handlers;
using TodoApp.Product.Domain.Exceptions;

namespace TodoApp.Product.Infrastructure.Handlers;

public class ExceptionHandler : IExceptionHandler
{
    private readonly ILogger<ExceptionHandler> _logger;

    public ExceptionHandler(ILogger<ExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async Task HandlerAsync(HttpContext httpContext, Exception exception)
    {
        switch (exception)
        {
            case ValidationFailedException validationFailedException:
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await httpContext.Response.WriteAsJsonAsync(validationFailedException.Errors);
                return;

            case ProductNotFoundException productNotFoundException:
                httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                _logger.LogError(productNotFoundException.Message);
                await httpContext.Response.WriteAsJsonAsync(new { Error = productNotFoundException.Message });
                return;
            default:
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                _logger.LogError("Something went wrong: ",exception.StackTrace);
                await httpContext.Response.WriteAsJsonAsync(new
                {
                    Error = "Unexpected error. Please contact the support."
                });
                return;
        }
    }
}