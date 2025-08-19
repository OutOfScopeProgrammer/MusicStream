using Microsoft.AspNetCore.Mvc;
using Music.API.Interfaces;
using MusicStream.Application.Interfaces;
using MusicStream.Application.Interfaces.Repositories;
using MusicStream.Domain.Entities;

namespace Music.API.MinimalApis;

public record CreateMusicDto(string Title, string Description, string SingerId, IFormFile File);

public class UserEndpoints : IEndpoint
{
    public IEndpointRouteBuilder Register(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/")
                        .MapGroup("v1");


        group.MapPost("music", async
        (IWebHostEnvironment env, [FromForm] CreateMusicDto dto, IMusicChannel channel, [FromServices] IMusicRepository musicRepository) =>
        {
            var ext = Path.GetExtension(dto.File.FileName);
            var fileName = Path.GetFileNameWithoutExtension(dto.File.FileName);
            var uploadPath = Path.Combine(env.WebRootPath, "Temp");
            Directory.CreateDirectory(uploadPath);
            var storedName = $"{Guid.NewGuid()}{ext}";
            var fullPath = Path.Combine(uploadPath, storedName);
            using var fileStream = File.Create(fullPath);
            await dto.File.CopyToAsync(fileStream);

            var music = new MusicStream.Domain.Entities.Music();
            music.Title = dto.Title;
            music.Description = dto.Description;
            music.SingerId = music.SingerId;
            musicRepository.AddMusic(music);
            await musicRepository.SaveChangesAsync(CancellationToken.None);
            await channel.SendAsync(new(fullPath, env.WebRootPath, fileName, music.Id));
            return Results.Ok("File went to background service");

        }).DisableAntiforgery();


        return group;
    }
}
