using System.Reflection;
using Music.API.Interfaces;

namespace Music.API.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddApiLayer(this IServiceCollection services)
    {

    }

    public static void AddEndpoints(this IEndpointRouteBuilder app)
    {
        var iEndpoint = typeof(IEndpoint);
        var endpoints = Assembly.GetExecutingAssembly()
        .GetTypes()
        .Where(t => t is { IsClass: true, IsAbstract: false } & iEndpoint.IsAssignableFrom(t));
        foreach (var item in endpoints)
        {
            var instance = Activator.CreateInstance(item) as IEndpoint;
            var route = instance?.Register(app);
        }

    }
}
