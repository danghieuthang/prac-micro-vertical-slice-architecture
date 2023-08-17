using MediatR;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Product.API.Features.Create;
using TodoApp.Product.API.Features.Detail;
using TodoApp.Product.API.Features.List;
using TodoApp.Product.API.Features.Update;

namespace TodoApp.Product.API.Features;

public static class ProductEndpoint
{
    public static async Task<IResult> GetAsync(
       [FromServices] IMediator mediator,
       CancellationToken cancellationToken = default)
    {

        var response = await mediator.Send(new ProductListQuery(), cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        return Results.Ok(response);
    }

    public static async Task<IResult> GetByIdAsync(
       [FromServices] IMediator mediator,
       [FromRoute] Guid id,
       CancellationToken cancellationToken = default)
    {

        var response = await mediator.Send(new ProductDetailQuery(id), cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        return Results.Ok(response);
    }

    public static async Task<IResult> PostAsync(
     [FromServices] IMediator mediator,
     [FromBody] ProductCreateCommand command,
     CancellationToken cancellationToken = default)
    {

        var response = await mediator.Send(command, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        return Results.Ok(response);
    }

    public static async Task<IResult> PutAsync(
     [FromServices] IMediator mediator,
     [FromBody] ProductUpdateCommand command,
     CancellationToken cancellationToken = default)
    {

        var response = await mediator.Send(command, cancellationToken)
            .ConfigureAwait(continueOnCapturedContext: false);

        return Results.Ok(response);
    }
}
