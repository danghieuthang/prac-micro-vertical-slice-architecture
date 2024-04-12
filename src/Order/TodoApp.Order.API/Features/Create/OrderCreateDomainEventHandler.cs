using MassTransit;
using MediatR;
using TodoApp.Messaging.Contracts;
using TodoApp.Order.Domain.Events;

namespace TodoApp.Order.API.Features.Create;

public class OrderCreateDomainEventHandler : INotificationHandler<OrderCreateDomainEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public OrderCreateDomainEventHandler(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(OrderCreateDomainEvent notification, CancellationToken cancellationToken)
    {
        var items = notification.Items.Select(y => new OrderCreatedIntegrationEventItem(y.ProductId, y.Quantity)).ToList();
        var integrationEvent = new OrderCreatedIntegrationEvent(notification.OrderId, items);
        await _publishEndpoint.Publish(integrationEvent, cancellationToken);
    }
}