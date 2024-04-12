using MassTransit;
using TodoApp.Infrastructure.Core.ServiceInvocation.MassTransit;

namespace TodoApp.Product.API.Features.Update;

public class ProductOderConsumerConfiguration : IConsumerConfiguration
{
    public void ConfigureConsumers(IRabbitMqBusFactoryConfigurator rabbitmq, IBusRegistrationContext context)
    {
        rabbitmq.ReceiveEndpoint("product-order", configurator =>
        {
            configurator.ConfigureConsumer<OrderCreateConsumer>(context);
        });
    }

    public void ConfigureMassTransit(IBusRegistrationConfigurator configurator)
    {
        configurator.AddConsumer<OrderCreateConsumer>();
    }
}
