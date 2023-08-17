using System.Reflection;
using EFCoreSecondLevelCacheInterceptor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TodoApp.Infrastructure.Core.Persistence;

namespace TodoApp.Infrastructure.Core.Extensions;
public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddPostgreDbContext<TDbContext>(
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

        var maxRetryCount = configuration.GetValue<int>("MySql:MaxRetryCount");
        var enableSecondLevelCache = configuration.GetValue<bool>("MySql:EnableSecondLevelCache");

        if (enableSecondLevelCache)
        {
            services.AddEFSecondLevelCache(options =>
                options.UseMemoryCacheProvider().DisableLogging(value: true)
            );
        }

        services.AddDbContext<TDbContext>((provider, optionsBuilder) =>
        {
            optionsBuilder.UseNpgsql(connectionString, options =>
            {
                options.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
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
