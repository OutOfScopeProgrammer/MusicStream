using Microsoft.AspNetCore.Mvc;
using Music.API.Interfaces;
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
        group.MapPost("users", () =>
        {
            return Results.Ok("User works");

        });
        group.MapPut("users", () =>
        {
            return Results.Ok();

        });

        group.DisableAntiforgery();

        return group;
    }
}
