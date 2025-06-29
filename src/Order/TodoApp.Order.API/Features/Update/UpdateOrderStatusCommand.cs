using MediatR;
using TodoApp.Order.Domain.Entities;

namespace TodoApp.Order.API.Features.Update;

public class UpdateOrderStatusCommand : IRequest<bool>
{
    public Guid OrderId { get; set; }
    public OrderStatus Status { get; set; }
}
