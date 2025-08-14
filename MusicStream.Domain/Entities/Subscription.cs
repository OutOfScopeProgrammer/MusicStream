namespace MusicStream.Domain.Entities;

public class Subscription
{
    public Guid Id { get; set; }
    public List<Playlist> Playlists { get; set; } = [];
    public User User { get; set; } = new();
}
