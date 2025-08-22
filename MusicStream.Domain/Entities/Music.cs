using MusicStream.Domain.Common;

namespace MusicStream.Domain.Entities;

public class Music : Auditable
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string StreamUrl { get; set; } = string.Empty;
    public List<Playlist> Playlist { get; set; } = [];
    public bool IsAvailable { get; set; }


    public static Music Create(string title, string description, bool isAvailable, string streamUrl)
        => new()
        {
            Title = title,
            Description = description,
            IsAvailable = isAvailable,
            StreamUrl = streamUrl
        };

}
