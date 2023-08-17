using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using MediatR.NotificationPublishers;
using TodoApp.Application.Core.Behaviors;
using TodoApp.Application.Core.Extensions;
using TodoApp.Application.Core.Middlewares;
using TodoApp.Infrastructure.Core.Extensions;
using TodoApp.Infrastructure.Core.Handlers;
using TodoApp.Product.API.Features;
using TodoApp.Product.API.Infrastructure;
using TodoApp.Product.API.Infrastructure.Handlers;

var builder = WebApplication.CreateBuilder(args);

var transactionsAssembly = typeof(Program).Assembly;
var transactionsAssemblyName = transactionsAssembly.GetName();

builder.Logging.ConfigureSerilogForOpenTelemetry();

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddPostgreDbContext<ProductDbContext>(
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
    .AddValidator(transactionsAssembly)
    .AddSingleton<IExceptionHandler, ExceptionHandler>()
    .AddUnitOfWork<ProductDbContext>()
    .AddOpenTelemetryConfiguration(
        serviceName: "product",
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

app.MapGet("/api/products", ProductEndpoint.GetAsync);
app.MapGet("/api/products/{id}", ProductEndpoint.GetByIdAsync);
app.MapPost("/api/products", ProductEndpoint.PostAsync);
app.MapPut("/api/products", ProductEndpoint.PutAsync);

await app.Services.ApplyMigrationsAsync<ProductDbContext>();

app.Run();


[ExcludeFromCodeCoverage]
public partial class Program
{
    protected Program()
    { }
}