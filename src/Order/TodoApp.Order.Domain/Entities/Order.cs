using TodoApp.Domain.Core;

namespace TodoApp.Order.Domain.Entities;
public class Order : IEntity<Guid>, ICreatableEntity, IModifiableEntity
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
}
