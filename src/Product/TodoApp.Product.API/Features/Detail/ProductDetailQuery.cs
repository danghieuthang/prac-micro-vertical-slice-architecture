using MediatR;

namespace TodoApp.Product.API.Features.Detail;

public record ProductDetailQuery(Guid Id) : IRequest<ProductDetailResponse>;
