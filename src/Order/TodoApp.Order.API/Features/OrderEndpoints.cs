using MediatR;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Order.API.Features.Create;
using TodoApp.Order.API.Features.Update;
using TodoApp.Order.API.Features.History;
using TodoApp.Order.API.Features.Cancel;
using TodoApp.Order.Domain.Entities;

namespace TodoApp.Order.API.Features;

public static class OrderEndpoints
{
    public static IEndpointRouteBuilder MapOrderApiRoutes(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("/api/v1/orders", OrderEndpoints.PostAsync);
        builder.MapPut("/api/v1/orders/{id}/status", OrderEndpoints.UpdateStatusAsync);
        builder.MapGet("/api/v1/orders/history", OrderEndpoints.GetOrderHistoryAsync);
        builder.MapPost("/api/v1/orders/{id}/cancel", OrderEndpoints.CancelOrderAsync);
        return builder;
    }

    public static async Task<IResult> PostAsync(
    [FromServices] IMediator mediator,
    [FromBody] OrderCreateCommand command,
    CancellationToken cancellationToken = default)
    {

        var response = await mediator.Send(command, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        return Results.Ok(response);
    }

    // Update order status
    public static async Task<IResult> UpdateStatusAsync(
        [FromServices] IMediator mediator,
        [FromRoute] Guid id,
        [FromBody] UpdateOrderStatusCommand command,
        CancellationToken cancellationToken = default)
    {
        command.OrderId = id;
        var response = await mediator.Send(command, cancellationToken).ConfigureAwait(false);
        return Results.Ok(response);
    }

    // Get order history
    public static async Task<IResult> GetOrderHistoryAsync(
        [FromServices] IMediator mediator,
        [FromQuery] Guid userId,
        [FromQuery] OrderStatus? status,
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to,
        CancellationToken cancellationToken = default)
    {
        var query = new GetOrderHistoryQuery(userId, status, from, to);
        var response = await mediator.Send(query, cancellationToken).ConfigureAwait(false);
        return Results.Ok(response);
    }

    // Cancel order and process refund
    public static async Task<IResult> CancelOrderAsync(
        [FromServices] IMediator mediator,
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var command = new CancelOrderCommand { OrderId = id };
        var response = await mediator.Send(command, cancellationToken).ConfigureAwait(false);
        return Results.Ok(response);
    }
}
