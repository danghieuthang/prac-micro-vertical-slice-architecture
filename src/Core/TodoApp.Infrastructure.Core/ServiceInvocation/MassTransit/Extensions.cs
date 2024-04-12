using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TodoApp.Infrastructure.Core.Scanners;

namespace TodoApp.Infrastructure.Core.ServiceInvocation.MassTransit;
public static class Extensions
{
    public static IServiceCollection AddRabbitMqMessaging(this IServiceCollection services,
  IConfiguration configuration,
  params Assembly[] consumerConfigurationAssemblies)
    {
        var host = configuration.GetValue<string>("RabbitMQ:Host");
        var virtualHost = configuration.GetValue<string>("RabbitMQ:VirtualHost") ?? "/";
        var username = configuration.GetValue<string>("RabbitMQ:Username") ?? "guest";
        var password = configuration.GetValue<string>("RabbitMQ:Password") ?? "guest";

        if (string.IsNullOrWhiteSpace(host))
        {
            throw new InvalidOperationException("RabbitMQ host was not found on configuration");
        }

        var consumerConfigurations = ConsumerConfigurationAssemblyScanner.Scan(consumerConfigurationAssemblies);

        services.AddMassTransit(configurator =>
        {
            foreach (var consumerConfiguration in consumerConfigurations)
            {
                consumerConfiguration.ConfigureMassTransit(configurator);
            }

            configurator.SetKebabCaseEndpointNameFormatter();
            configurator.UsingRabbitMq((context, rabbitmq) =>
            {
                rabbitmq.Host(host, virtualHost, hostConfigurator =>
                {
                    hostConfigurator.Username(username);
                    hostConfigurator.Password(password);
                });

                foreach (var consumerConfiguration in consumerConfigurations)
                {
                    consumerConfiguration.ConfigureConsumers(rabbitmq, context);
                }

                rabbitmq.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
