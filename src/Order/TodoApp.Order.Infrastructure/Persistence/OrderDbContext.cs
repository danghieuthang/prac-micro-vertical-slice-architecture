﻿using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TodoApp.Infrastructure.Core.Interceptors;
using TodoApp.Order.Domain.Entities;

namespace TodoApp.Order.Infrastructure.Persistence;

public class OrderDbContext : DbContext
{
    public OrderDbContext(DbContextOptions options) : base(options)
    {
        ChangeTracker.LazyLoadingEnabled = false;
        ChangeTracker.AutoDetectChangesEnabled = true;
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.Load("TodoApp.Infrastructure.Core"));
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderDbContext).Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(new ModifiableEntitySaveChangeInterceptor());
        optionsBuilder.AddInterceptors(new CreatableEntitySaveChangeInterceptor());
        base.OnConfiguring(optionsBuilder);
    }

    public DbSet<Domain.Entities.Order> Orders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }
}
