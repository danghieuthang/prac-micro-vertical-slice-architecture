﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Order.API.Features.Create;

namespace TodoApp.Order.API.Features;

public static class OrderEndpoints
{
    public static IEndpointRouteBuilder MapOrderApiRoutes(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("/api/v1/orders", OrderEndpoints.PostAsync);
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
}
