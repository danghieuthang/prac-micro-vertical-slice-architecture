using MediatR;
using TodoApp.Domain.Core;

namespace TodoApp.Product.API.Features.Update;

public record ProductOrderCommand(IEnumerable<ProductOderItem> Items) : IRequest, ICommand;
public record ProductOderItem(Guid Id, int Number);
