using System.Net.Sockets;
using Microsoft.AspNetCore.Mvc;
using Music.API.Api.EndpointFilters;
using Music.API.Helper;
using Music.API.Interfaces;
using MusicStream.Application.Interfaces.Repositories;
using MusicStream.Application.Services;

namespace Music.API.Api.Endpoints.MusicEndpoints;



public class MusicEndpoints : IEndpoint
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

        })
        .AddEndpointFilter<ValidationFilter<CreateMusicDto>>()
        .Produces<string>(StatusCodes.Status200OK, "application/json")
        .Produces(StatusCodes.Status404NotFound)
        .WithTags("Music")
        .WithDescription("ارسال موسیقی برای پردازش")
        .WithSummary("Create a music");


        group.MapGet("musics",
        async (IMusicRepository musicRepository, LinkGenerator linkGenerator, HttpContext context, CancellationToken cancellationToken) =>
        {
            var musics = await musicRepository.GetMusics(cancellationToken);
            if (musics is null)
                return Results.NotFound();
            var dtos = MusicDtoMapper.ToMusicDto(musics, linkGenerator, context);
            return Results.Ok(dtos);

        })
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .WithTags("Music")
        .WithDescription("لیست تمام موسیقی ها")
        .WithSummary("Get list of musics");


        return group.DisableAntiforgery();
    }
}
