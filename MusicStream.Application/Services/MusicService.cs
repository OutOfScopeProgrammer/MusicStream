using MusicStream.Application.Common;
using MusicStream.Application.Interfaces;
using MusicStream.Application.Interfaces.Repositories;
using MusicStream.Domain.Entities;

namespace MusicStream.Application.Services;


public class MusicService(IMusicRepository musicRepository, ISingerRepository singerRepository, IMusicChannel channel)
{

    public async Task<Response> CreateMusic(string title, string description
    , string fullPath, string webRootPath,
     string fileName, string singerId, CancellationToken cancellationToken)
    {
        Guid.TryParse(singerId, out var guidId);
        var singer = await singerRepository.GetSingerById(guidId, false, cancellationToken);
        if (singer is null)
            return Response.Failed(ErrorMessages.NotFound(singer));

        var music = Music.Create(title, description, singer);
        musicRepository.AddMusic(music);
        await musicRepository.SaveChangesAsync(CancellationToken.None);

        await channel.SendAsync(new(fullPath, webRootPath, fileName, music.Id));

        return Response.Succeed();
    }
}
