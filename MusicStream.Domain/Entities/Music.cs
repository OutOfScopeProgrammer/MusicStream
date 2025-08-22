using MusicStream.Domain.Common;

namespace MusicStream.Domain.Entities;

public class Music : Auditable
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Artist { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty;
    public string Duration { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public string StreamUrl { get; set; } = string.Empty;
    public List<Playlist> Playlist { get; set; } = [];
    public bool IsAvailable { get; set; }


    public static Music Create(string title, string artist, string date, string duration, string genre, bool isAvailable, string streamUrl)
        => new()
        {
            Title = title,
            Date = date,
            Duration = duration,
            Genre = genre,
            Artist = artist,
            IsAvailable = isAvailable,
            StreamUrl = streamUrl
        };

}
