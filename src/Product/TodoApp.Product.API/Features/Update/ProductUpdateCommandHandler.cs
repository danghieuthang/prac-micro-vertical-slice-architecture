using MediatR;
using Microsoft.EntityFrameworkCore;
using TodoApp.Product.Domain.Exceptions;
using TodoApp.Product.Infrastructure.Persistence;

namespace TodoApp.Product.API.Features.Update;

public class ProductUpdateCommandHandler(ProductDbContext productDbContext) : IRequestHandler<ProductUpdateCommand, Guid>
{
    public async Task<Guid> Handle(ProductUpdateCommand request, CancellationToken cancellationToken)
    {
        var product = await productDbContext.Products.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken).ConfigureAwait(false);
        if (product == null)
        {
            throw new ProductNotFoundException($"Product not found {request.Id}");
        }
        product.Quantity = request.Quantity;
        product.Name = request.Name;
        product.Price = request.Price;
        productDbContext.Update(product);
        return product.Id;
    }
}
