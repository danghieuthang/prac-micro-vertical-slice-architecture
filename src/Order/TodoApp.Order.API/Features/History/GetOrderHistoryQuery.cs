using MediatR;
using TodoApp.Order.Domain.Entities;

namespace TodoApp.Order.API.Features.History;

public record GetOrderHistoryQuery(
    Guid UserId,
    OrderStatus? Status,
    DateTime? From,
    DateTime? To
) : IRequest<List<OrderHistoryItem>>;

public record OrderHistoryItem(
    Guid OrderId,
    DateTime CreateAt,
    OrderStatus Status,
    decimal OrderTotal
);
