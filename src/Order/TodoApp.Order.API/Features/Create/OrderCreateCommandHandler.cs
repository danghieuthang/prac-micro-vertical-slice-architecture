using MediatR;
using TodoApp.Order.API.Infrastructure;
using TodoApp.Order.Domain.Entities;

namespace TodoApp.Order.API.Features.Create;

public class OrderCreateCommandHandler : IRequestHandler<OrderCreateCommand, Guid>
{
    private readonly OrderDbContext _orderDbContext;

    public OrderCreateCommandHandler(OrderDbContext orderDbContext)
    {
        _orderDbContext = orderDbContext;
    }

    public Task<Guid> Handle(OrderCreateCommand request, CancellationToken cancellationToken)
    {
        var order = new order.Order(request.UserId, request.Items.Select(y => new OrderDetail(y.ProductId, y.ProductName, y.UnitPrice, y.Quantity)).ToList());
        _orderDbContext.Orders.Add(order);
        return Task.FromResult(order.Id);
    }
}
