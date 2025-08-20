using MusicStream.Domain.Common;

namespace MusicStream.Domain.Entities;

public class Music : Auditable
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string StreamUrl { get; set; } = string.Empty;
    public List<Playlist> Playlist { get; set; } = [];

    public Singer? Singer { get; set; } = new();
    public Guid? SingerId { get; set; }

    public static Music Create(string title, string description, Singer singer)
        => new()
        {
            Title = title,
            Description = description,
            Singer = singer,
            SingerId = singer.Id
        };

}
