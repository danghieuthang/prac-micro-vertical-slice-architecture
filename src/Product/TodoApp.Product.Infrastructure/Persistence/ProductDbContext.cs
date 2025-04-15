using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TodoApp.Infrastructure.Core.Interceptors;

namespace TodoApp.Product.Infrastructure.Persistence;

public class ProductDbContext : DbContext
{
    public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
    {
    }

    public DbSet<Product.Domain.Entities.Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.Load("TodoApp.Infrastructure.Core"));
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductDbContext).Assembly);
        base.OnModelCreating(modelBuilder);

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(new ModifiableEntitySaveChangeInterceptor());
        optionsBuilder.AddInterceptors(new CreatableEntitySaveChangeInterceptor());
    }
}
