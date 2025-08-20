using MusicStream.Domain.Common;
using MusicStream.Domain.Enums;

namespace MusicStream.Domain.Entities;

public class Subscription : Auditable
{
    public Subscription() { }
    public Guid Id { get; set; }
    public SubscriptionType SubscriptionType { get; set; }
    public int PlaylistLimit { get; set; }
    public List<Playlist> Playlists { get; set; } = [];
    public User User { get; set; } = new();
    public Guid UserId { get; set; }


    public static Subscription Create(SubscriptionType type)
    {
        var sub = new Subscription();
        var limit = type switch
        {
            SubscriptionType.Free => 0,
            SubscriptionType.Basic => 2,
            SubscriptionType.Premium => 10,
            _ => 0
        };
        sub.PlaylistLimit = limit;
        return sub;
    }




    public bool TryAddPlaylist(Playlist playList)
    {
        if (Playlists.Count <= PlaylistLimit)
        {
            Playlists.Add(playList);
            return true;
        }
        return false;
    }



}
