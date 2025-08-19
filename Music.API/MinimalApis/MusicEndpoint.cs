using Microsoft.AspNetCore.Mvc;
using Music.API.Helper;
using Music.API.Interfaces;
using MusicStream.Application.Services;

namespace Music.API.MinimalApis;

public record CreateMusicDto(string Title, string Description, string SingerId, IFormFile File);

public class UserEndpoints : IEndpoint
{
    public IEndpointRouteBuilder Register(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/")
                        .MapGroup("v1");


        group.MapPost("music", async
        (IWebHostEnvironment env, [FromForm] CreateMusicDto dto,
        [FromServices] MusicService musicService, CancellationToken cancellationToken) =>
        {
            var (fullPath, fileName, uploadPath) = FileHelper.PrepareFileForSaving(dto.File.FileName, env.WebRootPath);
            Directory.CreateDirectory(uploadPath);

            using var fileStream = File.Create(fullPath);
            await dto.File.CopyToAsync(fileStream);
            var response = await musicService
            .CreateMusic(dto.Title, dto.Description, fullPath, env.WebRootPath,
             fileName, dto.SingerId, cancellationToken);

            return response.IsSuccess
            ? Results.Ok("File is in processing queue")
            : Results.NotFound(response.Error);

        }).Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
        // TODO: add validator


        return group.DisableAntiforgery();
    }
}
