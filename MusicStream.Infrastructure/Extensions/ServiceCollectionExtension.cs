using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MusicStream.Application.Interfaces;
using MusicStream.Application.Interfaces.Repositories;
using MusicStream.Infrastructure.Auth;
using MusicStream.Infrastructure.BackgroundServices;
using MusicStream.Infrastructure.Storages;
using MusicStream.Infrastructure.Persistence.Minio;
using MusicStream.Infrastructure.Persistence.Postgres;
using MusicStream.Infrastructure.Processors;
using MusicStream.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MusicStream.Infrastructure.Persistence.Postgres.Interceptors;

namespace MusicStream.Infrastructure.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMinio(configuration);
        services.AddPostgres(configuration);
        services.AddAuthServices(configuration);
        services.AddSingleton<IMusicStorage, MusicStorage>();
        services.AddScoped<IBucketManager, BucketManager>();
        services.AddSingleton<IMusicChannel, MusicChannel>();
        services.AddHostedService<MusicProcessingBackgroundService>();
        services.AddSingleton<MusicProcessor>();
        services.AddScoped<IMusicRepository, MusicRepository>();
        services.AddScoped<ISingerRepository, SingerRepository>();
    }

    private static void AddMinio(this IServiceCollection services, IConfiguration configuration)
    {
        var minioConfig = configuration.GetSection(nameof(MinioOption))
        ?? throw new Exception("mino config is null");

        services.AddOptions<MinioOption>().Bind(config: minioConfig);
        services.AddSingleton<MinioConnection>();
    }

    private static void AddPostgres(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Postgres")
        ?? throw new Exception("connetion string for postgres is null");

        services.AddScoped<IInterceptor, AuditableInterceptor>();
        services.AddDbContext<AppDbContext>((provider, option) =>
        {
            var interceptors = provider.GetServices<IInterceptor>()
            ?? throw new Exception("problem with Interceptors");
            option.AddInterceptors(interceptors);
            option.UseNpgsql(connectionString);
            option.EnableSensitiveDataLogging();
            option.EnableDetailedErrors();
        });
    }

    private static void AddAuthServices(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtOption = configuration.GetSection(nameof(JwtOption))
        ?? throw new Exception("Jwt setting are null");
        services.AddOptions<JwtOption>().Bind(jwtOption);

    }

}
