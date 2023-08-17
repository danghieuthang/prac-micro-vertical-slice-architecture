using MediatR;
using Microsoft.EntityFrameworkCore;
using TodoApp.Product.API.Infrastructure;
using TodoApp.Product.Domain.Exceptions;

namespace TodoApp.Product.API.Features.Update;

public class ProductUpdateCommandHandler : IRequestHandler<ProductUpdateCommand, Guid>
{
    private readonly ProductDbContext _productDbContext;

    public ProductUpdateCommandHandler(ProductDbContext productDbContext)
    {
        _productDbContext = productDbContext;
    }

    public async Task<Guid> Handle(ProductUpdateCommand request, CancellationToken cancellationToken)
    {
        var product = await _productDbContext.Products.FirstOrDefaultAsync(x => x.Id == request.Id).ConfigureAwait(false);
        if (product == null)
        {
            throw new ProductNotFoundException($"Product not found {request.Id}");
        }
        product.Quantity = request.Quantity;
        product.Name = request.Name;
        product.Price = request.Price;
        _productDbContext.Update(product);
        return product.Id;
    }
}
