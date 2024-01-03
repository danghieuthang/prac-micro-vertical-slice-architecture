using System.Text.Json;
using Dapr.Client;
using Microsoft.Extensions.DependencyInjection;

namespace TodoApp.Infrastructure.Core.ServiceInvocation.Dapr;

public static class Extensions
{
    public static IServiceCollection AddDaprClient(this IServiceCollection services)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        services.AddSingleton(options);
        services.AddDaprClient(client =>
        {
            client.UseJsonSerializationOptions(options);
        });

        return services;
    }
}