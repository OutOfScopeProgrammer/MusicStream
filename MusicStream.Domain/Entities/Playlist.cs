using MusicStream.Domain.Common;

namespace MusicStream.Domain.Entities;

public class Playlist : Auditable
{
    public Playlist() { }

    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public List<Music> Musics { get; set; } = [];
    public Subscription Subscription { get; set; } = new();
    public Guid SubscriptionId { get; set; }

    public void AddMusic(Music music) => Musics.Add(music);
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
