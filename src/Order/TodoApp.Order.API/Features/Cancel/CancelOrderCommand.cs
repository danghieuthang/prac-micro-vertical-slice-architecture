using MediatR;

namespace TodoApp.Order.API.Features.Cancel;

public class CancelOrderCommand : IRequest<bool>
{
    public Guid OrderId { get; set; }
}
