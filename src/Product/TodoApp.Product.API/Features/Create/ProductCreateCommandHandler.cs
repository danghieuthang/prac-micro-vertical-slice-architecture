using MediatR;
using TodoApp.Product.Infrastructure.Persistence;

namespace TodoApp.Product.API.Features.Create;

public class ProductCreateCommandHandler : IRequestHandler<ProductCreateCommand, Guid>
{
    private readonly ProductDbContext _productDbContext;

    public ProductCreateCommandHandler(ProductDbContext productDbContext)
    {
        _productDbContext = productDbContext;
    }

    public async Task<Guid> Handle(ProductCreateCommand request, CancellationToken cancellationToken)
    {
        var product = new TodoApp.Product.Domain.Entities.Product(request.Name, request.Quantity, request.Price);
        _productDbContext.Products.Add(product);
        return product.Id;
    }
}
