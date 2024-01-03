using MediatR;
using Microsoft.EntityFrameworkCore;
using TodoApp.Product.Domain.Entities;
using TodoApp.Product.Domain.Exceptions;
using TodoApp.Product.Infrastructure.Persistence;
using static Google.Rpc.Context.AttributeContext.Types;

namespace TodoApp.Product.API.Features.Update;


public class ProductOrderCommandHandler(ProductDbContext productDbContext) : IRequestHandler<ProductOrderCommand>
{
    public async Task Handle(ProductOrderCommand request, CancellationToken cancellationToken)
    {
        var products = await productDbContext.Products.Where(x => request.Items.Select(y => y.Id).Contains(x.Id)).ToListAsync(cancellationToken);
        if (products.Count != request.Items.Count())
        {
            throw new ProductNotFoundException("Product not found");
        }

        foreach (var product in products)
        {
            var item = request.Items.FirstOrDefault(x => x.Id == product.Id);
            product.Quantity -= item.Number;
        }
        productDbContext.Products.UpdateRange(products);
    }

}

