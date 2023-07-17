using Microsoft.AspNetCore.Http;

namespace TodoApp.Infrastructure.Core.Handlers;

public interface IExceptionHandler
{
    Task HandlerAsync(HttpContext httpContext, Exception exception);
}
