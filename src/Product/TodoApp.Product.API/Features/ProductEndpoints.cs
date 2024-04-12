using Dapr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Messaging.Contracts;
using TodoApp.Product.API.Features.Create;
using TodoApp.Product.API.Features.Detail;
using TodoApp.Product.API.Features.List;
using TodoApp.Product.API.Features.Update;

namespace TodoApp.Product.API.Features;

public static class ProductEndpoints
{
    public static IEndpointRouteBuilder MapProductApiRoutes(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("/api/v1/products", ProductEndpoints.GetAsync);
        builder.MapGet("/api/v1/products/{id}", ProductEndpoints.GetByIdAsync);
        builder.MapPost("/api/v1/products", ProductEndpoints.PostAsync);
        builder.MapPut("/api/v1/products", ProductEndpoints.PutAsync);

        return builder;
    }  
    
    //public static IEndpointRouteBuilder MapProductSubcribeHandlers(this IEndpointRouteBuilder builder)
    //{
    //    builder.MapSubscribeHandler();

    //    TopicOptions orderCreatedTopic = new()
    //    {
    //        PubsubName = "productpubsub",
    //        Name = "productordered",
    //        DeadLetterTopic = "productorderedDeadLetterTopic"
    //    };
    //    builder.MapPost("subcribe_productOrdered", async (OrderCreatedIntegrationEvent @event, ISender sender) =>
    //    {
    //        await sender.Send(
    //            new ProductOrderCommand(@event.Items.Select(x => new ProductOderItem(x.ProductId, x.Quantity))));
    //    }).WithTopic(orderCreatedTopic);

    //    return builder;
    //}

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
