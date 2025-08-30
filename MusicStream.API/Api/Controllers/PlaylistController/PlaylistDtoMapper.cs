using Music.API.Api.Controllers.MusicController;
using MusicStream.Domain.Entities;

namespace Music.API.Api.Controllers.PlaylistController;

public static class PlaylistDtoMapper
{
    public static PlaylistDto MapToDto(Playlist playlist, LinkGenerator linkGenerator, HttpContext context)
    {
        var musicsDto = MusicDtoMapper.ToMusicDto(playlist.Musics, linkGenerator, context);
        return new(playlist.Title, musicsDto);
    }
}
