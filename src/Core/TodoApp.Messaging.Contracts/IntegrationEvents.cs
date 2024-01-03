namespace TodoApp.Messaging.Contracts;

public record OrderCreatedIntegrationEvent(Guid OrderId, IEnumerable<OrderCreatedIntegrationEventItem> Items) ;

public record OrderCreatedIntegrationEventItem(Guid ProductId, int Quantity);
