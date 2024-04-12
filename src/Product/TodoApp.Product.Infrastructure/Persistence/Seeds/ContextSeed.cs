using Microsoft.EntityFrameworkCore;

namespace TodoApp.Product.Infrastructure.Persistence.Seeds;

internal static class ContextSeed
{
    public static void Seed(this ModelBuilder modelBuilder)
    {
        CreateProducts(modelBuilder);
    }

    private static void CreateProducts(ModelBuilder modelBuilder)
    {
        // fake a list of product
        var products = new List<Product.Domain.Entities.Product>
        {
            new Product.Domain.Entities.Product("Product 1", 10, 9.99m),
            new Product.Domain.Entities.Product("Product 2", 5, 19.99m),
            new Product.Domain.Entities.Product("Product 3", 3, 14.99m),
            new Product.Domain.Entities.Product("Product 4", 8, 7.99m),
            new Product.Domain.Entities.Product("Product 5", 2, 24.99m)
        };

        products.ForEach(x => x.CreateAt = DateTime.UtcNow);

        modelBuilder.Entity<Product.Domain.Entities.Product>().HasData(products);
    }
}