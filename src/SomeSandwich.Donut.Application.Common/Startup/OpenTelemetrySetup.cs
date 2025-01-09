using MassTransit.Logging;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace SomeSandwich.Donut.Application.Common.Startup;

/// <summary>
/// Provides extension methods to setup OpenTelemetry tracing.
/// </summary>
public static class OpenTelemetrySetup
{
    /// <summary>
    /// Configures OpenTelemetry tracing for the application.
    /// </summary>
    /// <param name="builder">The <see cref="OpenTelemetryBuilder"/> to configure.</param>
    /// <param name="serviceName">The name of the service to be used in tracing.</param>
    /// <param name="hasMongoDB">Indicates whether MongoDB instrumentation should be added.</param>
    /// <param name="hasRedis">Indicates whether Redis instrumentation should be added.</param>
    /// <param name="hasMassTransit"></param>
    /// <returns>The configured <see cref="OpenTelemetryBuilder"/>.</returns>
    public static OpenTelemetryBuilder Setup(this OpenTelemetryBuilder builder, string serviceName, bool hasMongoDB = false, bool hasRedis = false, bool hasMassTransit = false)
    {
        return builder
             .ConfigureResource(resource => resource.AddService(serviceName))
             .WithTracing(tracing =>
             {
                 tracing
                     .AddAspNetCoreInstrumentation()
                     .AddHttpClientInstrumentation();

                 if (hasMongoDB)
                 {
                     tracing.AddSource("MongoDB.Driver.Core.Extensions.DiagnosticSources");
                 }

                 if (hasRedis)
                 {
                     tracing.AddFusionCacheInstrumentation();
                 }

                 if (hasMassTransit)
                 {
                     tracing.AddSource(DiagnosticHeaders.DefaultListenerName);
                 }

                 tracing.AddOtlpExporter();
             })
             .WithMetrics(metric =>
             {
             });
    }
}
