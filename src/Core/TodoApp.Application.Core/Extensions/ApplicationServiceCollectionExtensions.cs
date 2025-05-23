﻿using System.Reflection;
using FluentValidation;
using MassTransit.Logging;
using MassTransit.Monitoring;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace TodoApp.Application.Core.Extensions;
public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddValidator(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddValidatorsFromAssemblies(assemblies);
        return services;
    }
    public static ILoggingBuilder AddOpenTelemetryLogs(this ILoggingBuilder builder,
        string serviceName,
       string? serviceNamespace = null,
       string? serviceVersion = null)
    {

        builder.ClearProviders();
        builder.AddOpenTelemetry(options =>
        {
            options.SetResourceBuilder(ResourceBuilder.CreateDefault()
                .AddService(
                    serviceName: serviceName,
                    serviceNamespace: serviceNamespace,
                    serviceVersion: serviceVersion,
                    autoGenerateServiceInstanceId: true
                ))
            .AddOtlpExporter();
        });

        return builder;
    }

    public static IServiceCollection AddOpenTelemetryConfiguration(
       this IServiceCollection services,
       string serviceName,
       string? serviceNamespace = null,
       string? serviceVersion = null)
    {
        services.AddOpenTelemetry()
            .ConfigureResource(resource => resource
                .AddService(
                    serviceName: serviceName,
                    serviceNamespace: serviceNamespace,
                    serviceVersion: serviceVersion,
                    autoGenerateServiceInstanceId: true
                )
                .AddEnvironmentVariableDetector()
            )
            .WithTracing(tracer => tracer
                .SetSampler<AlwaysOnSampler>()
                .AddAspNetCoreInstrumentation()
                //.AddEntityFrameworkCoreInstrumentation(options => options.SetDbStatementForText = true)
                .AddSource(DiagnosticHeaders.DefaultListenerName) // MassTransit activity source
                .AddOtlpExporter(oltpOption =>
                {
                    oltpOption.Endpoint = new Uri("http://localhost:4317");
                })
            )
            .WithMetrics(meter => meter
                .AddAspNetCoreInstrumentation()
                .AddRuntimeInstrumentation()
                .AddEventCountersInstrumentation(options => options
                    .AddEventSources(
                        "Microsoft.AspNetCore.Hosting",
                        "Microsoft.AspNetCore.Http.Connections",
                        "Microsoft-AspNetCore-Server-Kestrel",
                        "System.Net.Http",
                        "System.Net.NameResolution",
                        "System.Net.Security"
                    )
                )
                .AddMeter(InstrumentationOptions.MeterName) // MassTransit Meter
                .AddOtlpExporter()
            );

        return services;
    }
}
