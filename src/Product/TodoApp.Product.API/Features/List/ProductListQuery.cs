using MediatR;

namespace TodoApp.Product.API.Features.List;

public record ProductListQuery : IRequest<List<ProductListResponse>>
{
}
