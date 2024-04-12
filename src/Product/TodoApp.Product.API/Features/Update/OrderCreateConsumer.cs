using MassTransit;
using MediatR;
using TodoApp.Messaging.Contracts;

namespace TodoApp.Product.API.Features.Update;

public class OrderCreateConsumer : IConsumer<OrderCreatedIntegrationEvent>
{
    private readonly IMediator _mediator;

    public OrderCreateConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<OrderCreatedIntegrationEvent> context)
    {
        var message = context.Message;
        var command = new ProductOrderCommand(message.Items.Select(x => new ProductOderItem(x.ProductId, x.Quantity)));
        await _mediator.Send(command, context.CancellationToken).ConfigureAwait(false);
    }
}