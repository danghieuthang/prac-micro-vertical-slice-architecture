using MediatR;
using TodoApp.Domain.Core;

namespace TodoApp.Order.API.Features.Create;

public class OrderCreateCommand : IRequest<Guid>, ICommand
{
    public Guid UserId { get; set; }
    public List<OrderDetailItem> Items { get; set; }
}

public class OrderDetailItem
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public string ProductName { get; set; }
    public decimal UnitPrice { get; set; }
}