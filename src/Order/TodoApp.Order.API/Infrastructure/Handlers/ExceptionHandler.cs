using System.Net;
using TodoApp.Application.Core.Exceptions;
using TodoApp.Infrastructure.Core.Handlers;
using TodoApp.Order.Domain.Exceptions;

namespace TodoApp.Order.API.Infrastructure.Handlers;

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

            case OrderNotFoundException orderNotFoundException:
                httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                await httpContext.Response.WriteAsJsonAsync(new
                {
                    Error = orderNotFoundException.Message
                });
                return;
            default:
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await httpContext.Response.WriteAsJsonAsync(new { Error = "Unexpected error. Please contact the support." });
                return;
        }
    }
}
