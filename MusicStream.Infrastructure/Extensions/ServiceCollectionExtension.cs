using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MusicStream.Application.Interfaces;
using MusicStream.Infrastructure.Files;
using MusicStream.Infrastructure.Persistence.Minio;

namespace MusicStream.Infrastructure.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMinio(configuration);
        services.AddScoped<IFileStorage, FileStorage>();
        services.AddScoped<IBucketManager, BucketManager>();
    }

    private static void AddMinio(this IServiceCollection services, IConfiguration configuration)
    {
        var minioConfig = configuration.GetSection("minio");
        services.AddOptions<MinioOption>().Bind(config: minioConfig);
        services.AddSingleton<MinioConnection>();
    }

}
