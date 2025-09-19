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
using MusicStream.Application.Interfaces.Auth;
using Microsoft.AspNetCore.Identity;
using MusicStream.Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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

        services.AddSingleton<FFProcessor>();
        services.AddSingleton<MusicFileProcessor>();
        services.AddScoped<ISeeder, Seeder>();

        services.AddScoped<IMusicRepository, MusicRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
        services.AddScoped<IPlayListRepository, PlaylistRepository>();
        services.AddScoped<RefreshTokenRepository>();
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
        ?? throw new Exception("connection string problem");


        services.AddScoped<IInterceptor, AuditableInterceptor>();
        services.AddDbContext<AppDbContext>((provider, options) =>
        {
            var interceptors = provider.GetServices<IInterceptor>()
                               ?? throw new Exception("problem with interceptors");
            options.AddInterceptors(interceptors);
            options.UseNpgsql(connectionString);
            options.LogTo(_ => { }, LogLevel.None);

            // Enable detailed errors only in development
            // options.EnableSensitiveDataLogging();
            // options.EnableDetailedErrors();

        });
    }

    private static void AddAuthServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOption>(configuration.GetSection(nameof(JwtOption)));
        services.AddSingleton<ITokenGenerator, TokenGenerator>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

    }

}
