using System.Net;
using Microsoft.AspNetCore.Http;
using TodoApp.Application.Core.Exceptions;
using TodoApp.Infrastructure.Core.Handlers;
using TodoApp.Product.Domain.Exceptions;

namespace TodoApp.Product.Infrastructure.Handlers;

public class ExceptionHandler : IExceptionHandler
{
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
                await httpContext.Response.WriteAsJsonAsync(new
                {
                    Error = productNotFoundException.Message
                });
                return;
            default:
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await httpContext.Response.WriteAsJsonAsync(new { Error = "Unexpected error. Please contact the support." });
                return;
        }
    }
}
