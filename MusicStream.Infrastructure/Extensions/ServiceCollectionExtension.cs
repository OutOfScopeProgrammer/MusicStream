using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MusicStream.Application.Interfaces;
using MusicStream.Infrastructure.BackgroundServices;
using MusicStream.Infrastructure.Files;
using MusicStream.Infrastructure.Persistence.Minio;
using MusicStream.Infrastructure.Persistence.Postgres;
using MusicStream.Infrastructure.Processors;

namespace MusicStream.Infrastructure.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMinio(configuration);
        services.AddPostgres(configuration);
        services.AddScoped<IMusicStorage, MusicStorage>();
        services.AddScoped<IBucketManager, BucketManager>();
        services.AddSingleton<IMusicChannel, MusicChannel>();
        services.AddHostedService<MusicProcessingBackgroundService>();
        services.AddSingleton<MusicProcessor>();
    }

    private static void AddMinio(this IServiceCollection services, IConfiguration configuration)
    {
        var minioConfig = configuration.GetSection("minio")
        ?? throw new Exception("mino config is null");

        services.AddOptions<MinioOption>().Bind(config: minioConfig);
        services.AddSingleton<MinioConnection>();
    }

    private static void AddPostgres(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Postgres")
        ?? throw new Exception("connetion string for postgres is null");


        services.AddDbContext<AppDbContext>(option =>
        {
            option.UseNpgsql(connectionString);
            option.EnableSensitiveDataLogging();
            option.EnableDetailedErrors();
        });
    }

}
