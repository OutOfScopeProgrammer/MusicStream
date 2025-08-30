
namespace Music.API.Api.Controllers.MusicController;

public static class MusicDtoMapper
{

    public static MusicDto ToMusicDto(MusicStream.Domain.Entities.Music music, LinkGenerator linkGenerator, HttpContext context)
    {
        //Assumption: the stream url is always in this format : musicId/filename
        var parts = music.StreamUrl.Split("/");
        var streamUrl = linkGenerator.GetUriByName(context, "Stream",
         new { musicId = parts[0], fileName = parts[1] });
        if (streamUrl is null)
            throw new Exception("generating streamUrl failed");
        var dto = new MusicDto(music.Title, music.Artist, streamUrl);
        ;
        return dto;
    }

    public static List<MusicDto> ToMusicDto(List<MusicStream.Domain.Entities.Music> musics, LinkGenerator linkGenerator, HttpContext context)
    => musics.Select(m => ToMusicDto(m, linkGenerator, context)).ToList();

}
