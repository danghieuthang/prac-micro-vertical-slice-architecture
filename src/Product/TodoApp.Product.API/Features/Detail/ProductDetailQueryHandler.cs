using MediatR;
using Microsoft.EntityFrameworkCore;
using TodoApp.Product.API.Infrastructure;
using TodoApp.Product.Domain.Exceptions;

namespace TodoApp.Product.API.Features.Detail;

public class ProductDetailQueryHandler : IRequestHandler<ProductDetailQuery, ProductDetailResponse>
{
    private readonly ProductDbContext _productDbContext;

    public ProductDetailQueryHandler(ProductDbContext productDbContext)
    {
        _productDbContext = productDbContext;
    }

    public async Task<ProductDetailResponse> Handle(ProductDetailQuery request, CancellationToken cancellationToken)
    {
        var product = await _productDbContext.Products
            .Where(x => x.Id == request.Id)
            .Select(x => new ProductDetailResponse(x.Id, x.Name, x.Price, x.Quantity, x.CreateAt, x.ModifiedAt))
            .FirstOrDefaultAsync().ConfigureAwait(false);
        return product == null ? throw new ProductNotFoundException($"Product not found {request.Id}") : product;
    }
}
