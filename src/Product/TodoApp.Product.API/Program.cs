using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Dapr;
using MediatR;
using MediatR.NotificationPublishers;
using TodoApp.Application.Core.Behaviors;
using TodoApp.Application.Core.Extensions;
using TodoApp.Application.Core.Middlewares;
using TodoApp.Infrastructure.Core.Extensions;
using TodoApp.Infrastructure.Core.Handlers;
using TodoApp.Infrastructure.Core.ServiceInvocation.MassTransit;
using TodoApp.Messaging.Contracts;
using TodoApp.Product.API.Features;
using TodoApp.Product.API.Features.Update;
using TodoApp.Product.Infrastructure.Handlers;
using TodoApp.Product.Infrastructure.Persistence;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

Assembly productAssembly = typeof(Program).Assembly;
AssemblyName productAssemblyName = productAssembly.GetName();

builder.Logging.AddOpenTelemetryLogs("product-api",
        "todo-app",
        productAssemblyName.Version?.ToString() ?? null);

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddPostgresDbContext<ProductDbContext>(
        builder.Configuration,
        builder.Configuration.GetConnectionString(nameof(ProductDbContext)),
        ServiceLifetime.Scoped,
        Assembly.Load("ToDoApp.Infrastructure.Core")
    )
    .AddMediatR(configuration =>
    {
        configuration.RegisterServicesFromAssemblyContaining<Program>();
        configuration.AddOpenBehavior(typeof(LoggingBehavior<,>));
        configuration.AddOpenBehavior(typeof(ValidationBehavior<,>));
        configuration.AddOpenBehavior(typeof(ResilientTransactionBehavior<,>));
        configuration.NotificationPublisherType = typeof(TaskWhenAllPublisher);
    })
    .AddValidator(productAssembly)
    .AddSingleton<IExceptionHandler, ExceptionHandler>()
    .AddUnitOfWork<ProductDbContext>()
    .AddRabbitMqMessaging(builder.Configuration, productAssembly)
    .AddOpenTelemetryConfiguration(
        "product-api",
        "todo-app",
        productAssemblyName.Version?.ToString() ?? null
    );

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseRouting();

app.Map("/", () => Results.Redirect("/swagger"));
app.MapProductApiRoutes();
//app.MapProductSubcribeHandlers();

await app.Services.ApplyMigrationsAsync<ProductDbContext>();

app.Run();


[ExcludeFromCodeCoverage]
public partial class Program
{
    protected Program()
    {
    }
}