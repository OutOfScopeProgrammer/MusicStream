using MusicStream.Domain.Common;

namespace MusicStream.Domain.Entities;

public class Singer : Auditable
{
    public Singer() { }

    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public List<Music> Musics { get; set; } = [];

    public static Singer Create(string firstName, string lastName)
        => new()
        {
            FirstName = firstName,
            LastName = lastName
        };

    public void AddMusic(Music music) => Musics.Add(music);

}
