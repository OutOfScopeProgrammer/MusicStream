namespace MusicStream.Domain.Entities;

public class Playlist
{
    public Guid Id { get; set; }
    public List<Music> Musics { get; set; } = [];

}
