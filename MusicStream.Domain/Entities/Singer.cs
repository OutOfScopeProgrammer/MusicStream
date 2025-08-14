namespace MusicStream.Domain.Entities;

public class Singer
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public List<Music> Musics { get; set; } = [];

}
