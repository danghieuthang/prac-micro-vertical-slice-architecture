using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using MediatR.NotificationPublishers;
using TodoApp.Application.Core.Behaviors;
using TodoApp.Application.Core.Extensions;
using TodoApp.Application.Core.Middlewares;
using TodoApp.Infrastructure.Core.Extensions;
using TodoApp.Infrastructure.Core.Handlers;
using TodoApp.Infrastructure.Core.ServiceInvocation.Dapr;
using TodoApp.Order.API.Features;
using TodoApp.Order.Infrastructure.Handlers;
using TodoApp.Order.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

var transactionsAssembly = typeof(Program).Assembly;
var transactionsAssemblyName = transactionsAssembly.GetName();

builder.Logging.ConfigureSerilogForOpenTelemetry();

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddPostgresDbContext<OrderDbContext>(
        builder.Configuration,
        builder.Configuration.GetConnectionString(nameof(OrderDbContext)),
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
    .AddValidator(transactionsAssembly)
    .AddSingleton<IExceptionHandler, ExceptionHandler>()
    .AddUnitOfWork<OrderDbContext>()
    .AddDaprClient()
    .AddOpenTelemetryConfiguration(
        serviceName: "order",
        serviceNamespace: "todo-app",
        serviceVersion: transactionsAssemblyName.Version?.ToString() ?? null
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
app.UseRouting();
app.MapOrderApiRoutes();

await app.Services.ApplyMigrationsAsync<OrderDbContext>();


app.Run();

[ExcludeFromCodeCoverage]
public partial class Program
{
    protected Program()
    { }
}