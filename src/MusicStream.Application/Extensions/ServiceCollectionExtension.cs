using Microsoft.Extensions.DependencyInjection;
using MusicStream.Application.Services;

namespace MusicStream.Application.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddApplicationLayer(this IServiceCollection services)
    {
        services.AddScoped<PlaylistService>();
    }
}
