using System.Reflection;
using EFCoreSecondLevelCacheInterceptor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using TodoApp.Infrastructure.Core.Persistence;

namespace TodoApp.Infrastructure.Core.Extensions;
public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddPostgresDbContext<TDbContext>(
       this IServiceCollection services,
       IConfiguration configuration,
       string? connectionString,
       ServiceLifetime? serviceLifetime,
       params Assembly[] interceptorsAssemblies) where TDbContext : DbContext
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException($"Connection string for {nameof(TDbContext)} was not found.");
        }

        var maxRetryCount = configuration.GetValue<int>("Postgres:MaxRetryCount");
        var enableSecondLevelCache = configuration.GetValue<bool>("Postgres:EnableSecondLevelCache");

        if (enableSecondLevelCache)
        {
            services.AddEFSecondLevelCache(options =>
                options.UseMemoryCacheProvider().ConfigureLogging(enable: false)
            );
        }

        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        dataSourceBuilder.EnableDynamicJson();
        var dataSource = dataSourceBuilder.Build();

        services.AddDbContext<TDbContext>((provider, optionsBuilder) =>
        {
            optionsBuilder.UseNpgsql(dataSource, options =>
            {
                options.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                options.EnableRetryOnFailure(maxRetryCount);

            });

            optionsBuilder.EnableSensitiveDataLogging();

            var interceptors = provider.ResolveEfCoreInterceptors(enableSecondLevelCache, interceptorsAssemblies);
            if (interceptors.Any())
            {
                optionsBuilder.AddInterceptors(interceptors);
            }
        }, serviceLifetime ?? ServiceLifetime.Scoped);

        return services;
    }

    public static IServiceCollection AddUnitOfWork<TDbContext>(this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
     where TDbContext : DbContext
    {
        services.TryAdd(new ServiceDescriptor(typeof(IUnitOfWork), typeof(UnitOfWork<TDbContext>), serviceLifetime));

        return services;
    }
}
