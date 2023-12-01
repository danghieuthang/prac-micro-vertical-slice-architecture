using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TodoApp.Infrastructure.Core.Extensions;
using TodoApp.Product.API.Infrastructure;

namespace TodoApp.Product.API.Test;

public abstract class BaseTest
{
    protected IServiceProvider ServiceProvider { get; private set; }

    protected void SetUp(string configurationPath = "settings.json")
    {
        IConfigurationRoot? configuration = new ConfigurationBuilder()
            .AddJsonFile(configurationPath)
            .Build();
        ServiceCollection serviceCollection = new();
        serviceCollection.AddUnitOfWork<ProductDbContext>();
        ServiceProvider = serviceCollection.BuildServiceProvider();
    }

    protected List<MasterData> InitializeMasterData()
    {
        return 
    }
}