using TodoApp.Domain.Core;

namespace TodoApp.Order.Domain.Entities;

public class OrderDetail : IEntity<Guid>, ICreatableEntity, IModifiableEntity
{
    public Guid Id { get; private set; }
    public DateTime CreateAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public decimal LineTotal => UnitPrice * Quantity;

    public Order Order { get; set; }

    public OrderDetail()
    {

    }

    public OrderDetail(Guid productId, string productName, decimal unitPrice, int quantity)
    {
        Id = Guid.NewGuid();
        ProductId = productId;
        ProductName = productName;
        UnitPrice = unitPrice;
        Quantity = quantity;
    }
}
