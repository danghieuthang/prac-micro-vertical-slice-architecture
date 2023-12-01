using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using MediatR.NotificationPublishers;
using TodoApp.Application.Core.Behaviors;
using TodoApp.Application.Core.Extensions;
using TodoApp.Application.Core.Middlewares;
using TodoApp.Infrastructure.Core.Extensions;
using TodoApp.Infrastructure.Core.Handlers;
using TodoApp.Product.API.Features;
using TodoApp.Product.Infrastructure.Handlers;
using TodoApp.Product.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

var productAssembly = typeof(Program).Assembly;
var productAssemblyName = productAssembly.GetName();

builder.Logging.ConfigureSerilogForOpenTelemetry();

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
    .AddOpenTelemetryConfiguration(
        serviceName: "product",
        serviceNamespace: "todo-app",
        serviceVersion: productAssemblyName.Version?.ToString() ?? null
    );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.MapProductApiRoutes();

await app.Services.ApplyMigrationsAsync<ProductDbContext>();

app.Run();


[ExcludeFromCodeCoverage]
public partial class Program
{
    protected Program()
    { }
}