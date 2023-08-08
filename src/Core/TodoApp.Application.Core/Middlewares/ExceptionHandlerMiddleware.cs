namespace TodoApp.Application.Core.Middlewares;
using Microsoft.AspNetCore.Http;
using TodoApp.Infrastructure.Core.Handlers;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IExceptionHandler _handler;

    public ExceptionHandlerMiddleware(RequestDelegate next, IExceptionHandler handler)
    {
        _next = next;
        _handler = handler;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            await _handler.HandlerAsync(context, ex).ConfigureAwait(false);
        }
    }

}
