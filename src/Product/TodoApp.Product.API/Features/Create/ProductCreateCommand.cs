using MediatR;
using TodoApp.Domain.Core;

namespace TodoApp.Product.API.Features.Create;

public record ProductCreateCommand(string Name, int Quantity, decimal Price) : IRequest<Guid>, ICommand;
