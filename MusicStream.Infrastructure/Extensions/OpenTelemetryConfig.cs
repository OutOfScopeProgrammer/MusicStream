using Microsoft.Extensions.DependencyInjection;
using MusicStream.Infrastructure.Observability;
using OpenTelemetry;
using OpenTelemetry.Metrics;

namespace MusicStream.Infrastructure.Extensions;

public static class OpenTelemetryConfig
{

    public static void AddOpenTelemetry(this IServiceCollection services)
    {
        var meterProvider = Sdk.CreateMeterProviderBuilder()
        .AddMeter(AppMeters.FFPROBEPROCESS)
        .AddMeter(AppMeters.FFMPEGSPROCESS)
        .AddMeter(AppMeters.MUSICBACKGROUNDSERVICE)
        .AddAspNetCoreInstrumentation()
        .AddRuntimeInstrumentation()
        .Build();

        services.AddSingleton(meterProvider);
    }
}
