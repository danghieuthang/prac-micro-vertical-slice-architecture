using MediatR;
using TodoApp.Domain.Core;

namespace TodoApp.Product.API.Features.Update;

public record ProductUpdateCommand(Guid Id, string Name, int Quantity, decimal Price) : IRequest<Guid>, ICommand;
