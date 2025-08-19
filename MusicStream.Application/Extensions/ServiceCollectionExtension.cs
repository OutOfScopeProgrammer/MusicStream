using Microsoft.Extensions.DependencyInjection;
using MusicStream.Application.Services;
using MusicStream.Domain.Entities;

namespace MusicStream.Application.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddApplicationLayer(this IServiceCollection services)
    {
        services.AddScoped<MusicService>();
    }
}
