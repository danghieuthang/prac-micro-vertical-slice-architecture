namespace TodoApp.Product.API.Features.Detail;

public record ProductDetailResponse(Guid Id, string Name, decimal Price, decimal Quantity, DateTime CreatedAt, DateTime? ModifiedAt);
