using MediatR;
using Microsoft.EntityFrameworkCore;
using TodoApp.Order.Domain.Entities;
using TodoApp.Order.Infrastructure.Persistence;

namespace TodoApp.Order.API.Features.Cancel;

public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand, bool>
{
    private readonly OrderDbContext _dbContext;
    public CancelOrderCommandHandler(OrderDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _dbContext.Orders.FirstOrDefaultAsync(x => x.Id == request.OrderId, cancellationToken);
        if (order == null) return false;
        if (order.Status == OrderStatus.Cancelled || order.Status == OrderStatus.Refunded)
            return false;
        order.Status = OrderStatus.Cancelled;
        order.ModifiedAt = DateTime.UtcNow;
        // TODO: Integrate with payment service for refund
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
