using MediatR;
using Microsoft.EntityFrameworkCore;
using TodoApp.Order.Domain.Entities;
using TodoApp.Order.Infrastructure.Persistence;

namespace TodoApp.Order.API.Features.Update;

public class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommand, bool>
{
    private readonly OrderDbContext _dbContext;
    public UpdateOrderStatusCommandHandler(OrderDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var order = await _dbContext.Orders.FirstOrDefaultAsync(x => x.Id == request.OrderId, cancellationToken);
        if (order == null) return false;
        order.Status = request.Status;
        order.ModifiedAt = DateTime.UtcNow;
        // TODO: Emit event here if needed
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
