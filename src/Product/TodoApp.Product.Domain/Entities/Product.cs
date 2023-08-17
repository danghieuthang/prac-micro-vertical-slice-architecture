using TodoApp.Domain.Core;

namespace TodoApp.Product.Domain.Entities;
public class Product : IEntity<Guid>, ICreatableEntity, IModifiableEntity
{
    public Guid Id { get; private set; }
    public string Name { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public DateTime CreateAt { get; set; }
    public DateTime? ModifiedAt { get; set; }

    public Product(string name, int quantity, decimal price)
    {
        Id = Guid.NewGuid();
        Name = name;
        Quantity = quantity;
        Price = price;
    }
}
