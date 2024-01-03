using MediatR;

namespace TodoApp.Order.Domain.Events;

public record OrderCreateDomainEvent(Guid OrderId, IEnumerable<OrderCreateDomainEventItem> Items) : INotification;

public record OrderCreateDomainEventItem(Guid ProductId, int Quantity);