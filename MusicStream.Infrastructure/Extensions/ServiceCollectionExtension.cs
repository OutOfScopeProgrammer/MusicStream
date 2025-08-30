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

namespace MusicStream.Infrastructure.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        services.AddMinio(configuration);
        services.AddPostgres(configuration, environment);
        services.AddAuthServices(configuration);
        services.AddSingleton<IMusicStorage, MusicStorage>();
        services.AddScoped<IBucketManager, BucketManager>();
        services.AddSingleton<IMusicChannel, MusicChannel>();
        services.AddHostedService<MusicProcessingBackgroundService>();

        services.AddSingleton<FFProcessor>();
        services.AddSingleton<MusicFileProcessor>();

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

    private static void AddPostgres(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {

        var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION")
                               ?? configuration.GetConnectionString("Postgres")
                               ?? throw new Exception("No DB connection string provided");

        var hostEnv = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
        var userEnv = Environment.GetEnvironmentVariable("DB_USER") ?? "postgres";
        var passwordEnv = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "password";
        var dbEnv = Environment.GetEnvironmentVariable("DB_NAME") ?? "MusicStream";

        var dockerConnectionString = $"Host={hostEnv};Port=5432;Username={userEnv};Password={passwordEnv};Database={dbEnv};SSL Mode=Disable;Trust Server Certificate=true;";

        var finalConnectionString = Environment.GetEnvironmentVariable("DB_CONNECTION") == null
                                    ? dockerConnectionString
                                    : connectionString;

        services.AddScoped<IInterceptor, AuditableInterceptor>();
        services.AddDbContext<AppDbContext>((provider, options) =>
        {
            var interceptors = provider.GetServices<IInterceptor>()
                               ?? throw new Exception("problem with interceptors");
            options.AddInterceptors(interceptors);
            options.UseNpgsql(finalConnectionString);

            if (environment.IsDevelopment())
            {
                // Enable detailed errors only in development
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            }
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
