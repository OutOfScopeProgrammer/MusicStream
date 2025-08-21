using MusicStream.Domain.Common;

namespace MusicStream.Domain.Entities;

public class Playlist : Auditable
{
    public Playlist() { }

    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public List<Music> Musics { get; set; } = [];
    public Subscription Subscription { get; set; } = new();
    public int MusicLimits { get; set; }
    public Guid SubscriptionId { get; set; }

    public string? TryAddMusic(Music music)
    {
        if (Musics.Count >= MusicLimits)
            return $"playlist is full.";
        else
        {
            Musics.Add(music);
            return string.Empty;
        }
    }
    public void RemoveMusic(Music music) => Musics.Remove(music);

    public void UpdateTitle(string title) => Title = title;
    public static Playlist Create(Subscription subscription, string title)
        => new()
        {
            Title = title,
            Subscription = subscription,
            SubscriptionId = subscription.Id
        };
}
