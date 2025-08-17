using Microsoft.AspNetCore.Mvc;
using Music.API.Interfaces;
using MusicStream.Application;
using MusicStream.Application.Interfaces;

namespace Music.API.MinimalApis;



public class UserEndpoints : IEndpoint
{
    public IEndpointRouteBuilder Register(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1");


        group.MapPost("music", async (IWebHostEnvironment env, [FromForm] IFormFile file, IMusicChannel channel) =>
        {
            var ext = Path.GetExtension(file.FileName);

            var uploadPath = Path.Combine(env.WebRootPath, "Temp");
            Directory.CreateDirectory(uploadPath);
            var storedName = $"{Guid.NewGuid()}{ext}";
            var fullPath = Path.Combine(uploadPath, storedName);
            using var fileStream = File.Create(fullPath);
            await file.CopyToAsync(fileStream);

            await channel.SendAsync(new MusicChannelMessage(fullPath, env.WebRootPath, storedName));
            return Results.Ok("File went to background service");

        }).DisableAntiforgery();

        group.MapGet("stream/{musicId}/{fileName}",
        async (string musicId, string fileName, IFileStorage fileStorage, HttpContext context) =>
        {
            Console.WriteLine(".......request");
            var contentType = fileName.EndsWith(".mpd") ? "application/dash+xml" :
                              fileName.EndsWith(".m4s") ? "application/iso.segment"
                              : "application/octet-stream";
            var key = $"{musicId}/{fileName}";
            var file = await fileStorage.DownloadFile("music-bucket", key);
            return Results.File(file, contentType, fileName);

        });
        return group;
    }
}
