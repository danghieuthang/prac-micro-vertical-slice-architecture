using Microsoft.Extensions.Logging;
using Serilog;

namespace TodoApp.Infrastructure.Core.Extensions;
public static class InfrastructureLoggingBuilderExtensions
{
    public static ILoggingBuilder ConfigureSerilogForOpenTelemetry(this ILoggingBuilder builder)
    {
        builder.ClearProviders();

        var logger = new LoggerConfiguration()
            .WriteTo.OpenTelemetry()
            .CreateLogger();

        Log.Logger = logger;

        builder.AddSerilog(logger);

        return builder;
    }
}