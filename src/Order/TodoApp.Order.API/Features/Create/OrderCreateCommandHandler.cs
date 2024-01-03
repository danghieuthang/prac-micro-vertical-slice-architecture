using MediatR;
using TodoApp.Order.Domain.Entities;
using TodoApp.Order.Infrastructure.Persistence;

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
        order.AddOrderCreatedIntegrationEvent();
        _orderDbContext.Orders.Add(order);
        return Task.FromResult(order.Id);
    }

}
