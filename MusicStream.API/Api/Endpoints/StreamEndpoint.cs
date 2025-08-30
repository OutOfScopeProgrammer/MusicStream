using Music.API.Interfaces;
using MusicStream.Application.Interfaces;

namespace Music.API.Api.Endpoints;

public class StreamEndpoint : IEndpoint
{
    public IEndpointRouteBuilder Register(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api")
                        .MapGroup("v1");


        group.MapGet("stream/{musicId}/{fileName}",
        async (string musicId, string fileName, IMusicStorage fileStorage, HttpContext context) =>
        {
            Console.WriteLine(".......request");
            var contentType = fileName.EndsWith(".mpd") ? "application/dash+xml" :
                              fileName.EndsWith(".m4s") ? "application/iso.segment"
                              : "application/octet-stream";
            var key = $"{musicId}/{fileName}";
            var file = await fileStorage.DownloadFile(key);
            return Results.File(file, contentType, fileName);

        })
        .WithName("Stream")
        .WithTags("Stream")
        .WithDescription("اندپویند استریم موسیقی")
        .WithSummary("Stream endpoint");
        group.DisableAntiforgery();
        return group;
    }


}
