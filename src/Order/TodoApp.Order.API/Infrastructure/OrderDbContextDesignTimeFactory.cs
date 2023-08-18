using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using TodoApp.Infrastructure.Core.Factories;

namespace TodoApp.Order.API.Infrastructure;

public class OrderDbContextDesignTimeFactory : IDesignTimeDbContextFactory<OrderDbContext>
{
    public OrderDbContext CreateDbContext(string[] args)
    {
        var configuration = ConfigurationFactory.CreateConfiguration();

        var connectionString = configuration.GetConnectionString(nameof(OrderDbContext));

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new Exception("Connection string not found.");
        }

        var builder = new DbContextOptionsBuilder<OrderDbContext>()
            .UseNpgsql(connectionString);

        return new OrderDbContext(builder.Options);
    }
}
