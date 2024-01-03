using Dapr.Client;
using MediatR;
using TodoApp.Messaging.Contracts;
using TodoApp.Order.Domain.Events;

namespace TodoApp.Order.API.Features.Create;

public class OrderCreateDomainEventHandler : INotificationHandler<OrderCreateDomainEvent>
{
    private readonly DaprClient _daprClient;

    public OrderCreateDomainEventHandler(DaprClient daprClient)
    {
        _daprClient = daprClient;
    }

    public async Task Handle(OrderCreateDomainEvent notification, CancellationToken cancellationToken)
    {
        var items = notification.Items.Select(y => new OrderCreatedIntegrationEventItem(y.ProductId, y.Quantity)).ToList();
        var integrationEvent = new OrderCreatedIntegrationEvent(notification.OrderId, items);
        await _daprClient.PublishEventAsync("productpubsub", "productordered", integrationEvent);
    }
}