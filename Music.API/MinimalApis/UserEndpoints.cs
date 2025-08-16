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

        group.MapGet("upload", async ([FromServices] IFileStorage fileStorage) =>
        {
            await fileStorage.UploadFile("bucket", "myFile", "Hello World");
            return Results.Ok("Uploaded");
        });
        group.MapGet("download", async ([FromServices] IFileStorage fileStorage) =>
        {
            var text = await fileStorage.DownloadFile("bucket", "myFile");
            return Results.Ok(text);
        });
        group.MapPost("music", async (IWebHostEnvironment env, [FromForm] IFormFile file, IMusicChannel channel) =>
        {
            var ext = Path.GetExtension(file.FileName);

            var uploadPath = Path.Combine(env.WebRootPath, "Temp");
            Directory.CreateDirectory(uploadPath);
            var storedName = $"{Guid.NewGuid()}{ext}";
            var fullPath = Path.Combine(uploadPath, storedName);
            using var fileStream = File.Create(fullPath);
            await file.CopyToAsync(fileStream);

            await channel.SendAsync(new ChannelDto(fullPath, env.WebRootPath, "MyMusic"));
            return Results.Ok("File went to background service");

        }).DisableAntiforgery();
        group.MapPut("users", () =>
        {
            return Results.Ok();

        });

        group.DisableAntiforgery();

        return group;
    }
}
