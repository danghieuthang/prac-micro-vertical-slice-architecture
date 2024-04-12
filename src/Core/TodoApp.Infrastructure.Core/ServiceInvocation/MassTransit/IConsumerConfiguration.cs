using MassTransit;

namespace TodoApp.Infrastructure.Core.ServiceInvocation.MassTransit;

public interface IConsumerConfiguration
{
    void ConfigureMassTransit(IBusRegistrationConfigurator configurator);

    void ConfigureConsumers(IRabbitMqBusFactoryConfigurator rabbitmq, IBusRegistrationContext context);
}