using MediatR;
using Microsoft.EntityFrameworkCore;
using TodoApp.Order.Domain.Entities;
using TodoApp.Order.Infrastructure.Persistence;

namespace TodoApp.Order.API.Features.History;

public class GetOrderHistoryQueryHandler : IRequestHandler<GetOrderHistoryQuery, List<OrderHistoryItem>>
{
    private readonly OrderDbContext _dbContext;
    public GetOrderHistoryQueryHandler(OrderDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<OrderHistoryItem>> Handle(GetOrderHistoryQuery request, CancellationToken cancellationToken)
    {
        var query = _dbContext.Orders.AsQueryable();
        query = query.Where(x => x.UserId == request.UserId);
        if (request.Status.HasValue)
            query = query.Where(x => x.Status == request.Status);
        if (request.From.HasValue)
            query = query.Where(x => x.CreateAt >= request.From);
        if (request.To.HasValue)
            query = query.Where(x => x.CreateAt <= request.To);
        return await query.Select(x => new OrderHistoryItem(x.Id, x.CreateAt, x.Status, x.OrderTotal)).ToListAsync(cancellationToken);
    }
}
