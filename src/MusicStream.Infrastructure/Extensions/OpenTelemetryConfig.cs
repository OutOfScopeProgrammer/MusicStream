using Microsoft.Extensions.DependencyInjection;
using MusicStream.Infrastructure.Observability;
using OpenTelemetry;
using OpenTelemetry.Metrics;

namespace MusicStream.Infrastructure.Extensions;

public static class OpenTelemetryConfig
{

    public static void AddOpenTelemetryConfiguration(this IServiceCollection services)
    {
        var meterProvider = Sdk.CreateMeterProviderBuilder()
        .AddMeter(FFMeter.FFMPEGSPROCESS)
        .AddAspNetCoreInstrumentation()
        .AddRuntimeInstrumentation()
        .Build();

        services.AddSingleton(meterProvider);
    }
}
