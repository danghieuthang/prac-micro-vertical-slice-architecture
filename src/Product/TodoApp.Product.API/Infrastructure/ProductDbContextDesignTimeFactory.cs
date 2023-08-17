using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using TodoApp.Infrastructure.Core.Factories;

namespace TodoApp.Product.API.Infrastructure;

public class ProductDbContextDesignTimeFactory : IDesignTimeDbContextFactory<ProductDbContext>
{
    public ProductDbContext CreateDbContext(string[] args)
    {
        var configuration = ConfigurationFactory.CreateConfiguration();

        var connectionString = configuration.GetConnectionString(nameof(ProductDbContext));

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new Exception("Connection string not found.");
        }

        var builder = new DbContextOptionsBuilder<ProductDbContext>()
            .UseNpgsql(connectionString);

        return new ProductDbContext(builder.Options);
    }
}
