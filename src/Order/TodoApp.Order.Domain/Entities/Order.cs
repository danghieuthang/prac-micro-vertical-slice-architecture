using TodoApp.Domain.Core;
using TodoApp.Messaging.Contracts;
using TodoApp.Order.Domain.Events;

namespace TodoApp.Order.Domain.Entities;
public class Order : AggregateRoot<Guid>, ICreatableEntity, IModifiableEntity
{
    public Guid Id { get; private set; }
    public DateTime CreateAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public Guid UserId { get; set; }
    public decimal OrderTotal { get; set; }

    public ICollection<OrderDetail> OrderDetails { get; set; }

    public Order()
    {

    }

    public Order(Guid userId, ICollection<OrderDetail> items)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        OrderDetails = items;
        OrderTotal = items.Sum(x => x.Quantity * x.UnitPrice);
    }

    public void AddOrderCreatedIntegrationEvent()
    {
        var items = OrderDetails.Select(x => new OrderCreateDomainEventItem(x.ProductId, x.Quantity));
        var @event = new OrderCreateDomainEvent(Id, items);
        AddDomainEvent(@event);
    }
}
