using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TodoApp.Infrastructure.Core.Interceptors;

namespace TodoApp.Product.API.Infrastructure;

public class ProductDbContext : DbContext
{
    public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
    {
        ChangeTracker.LazyLoadingEnabled = false;
        ChangeTracker.AutoDetectChangesEnabled = true;
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
    }

    public DbSet<TodoApp.Product.Domain.Entities.Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.Load("TodoApp.Infrastructure.Core"));
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductDbContext).Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.AddInterceptors(new CreatableEntitySaveChangeInterceptor());
        optionsBuilder.AddInterceptors(new ModifiableEntitySaveChangeInterceptor());
    }
}
