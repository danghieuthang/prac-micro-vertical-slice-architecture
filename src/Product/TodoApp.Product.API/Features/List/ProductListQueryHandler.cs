using MediatR;
using Microsoft.EntityFrameworkCore;
using TodoApp.Product.API.Infrastructure;

namespace TodoApp.Product.API.Features.List;

public class ProductListQueryHandler : IRequestHandler<ProductListQuery, List<ProductListResponse>>
{
    private readonly ProductDbContext _productDbContext;

    public ProductListQueryHandler(ProductDbContext productDbContext)
    {
        _productDbContext = productDbContext;
    }

    public async Task<List<ProductListResponse>> Handle(ProductListQuery request, CancellationToken cancellationToken)
    {
        var products = await _productDbContext
            .Products
            .Select(x => new ProductListResponse(x.Id, x.Name, x.Price))
            .ToListAsync();
        return products;
    }
}
