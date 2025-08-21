using MusicStream.Domain.Common;

namespace MusicStream.Domain.Entities;

public class Playlist : Auditable
{
    public Playlist() { }

    public Guid Id { get; set; }
    public List<Music> Musics { get; set; } = [];
    public Subscription Subscription { get; set; } = new();
    public Guid SubscriptionId { get; set; }

    public void AddMusic(Music music) => Musics.Add(music);
    public static Playlist Create(Subscription subscription)
        => new()
        {
            Subscription = subscription,
            SubscriptionId = subscription.Id
        };
}
