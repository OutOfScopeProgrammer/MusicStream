using System.Text.Json.Serialization;
using MusicStream.Domain.Common;
using MusicStream.Domain.Enums;

namespace MusicStream.Domain.Entities;

public class Subscription : Auditable
{
    public Subscription() { }
    public Guid Id { get; set; }
    public SubscriptionType SubscriptionType { get; set; }
    public int PlaylistLimit { get; set; }
    [JsonIgnore]
    public List<Playlist> Playlists { get; set; } = [];

    [JsonIgnore]
    public User? User { get; set; }
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



    public Response TryAddPlaylist(Playlist playList)
    {
        if (Playlists.Count < PlaylistLimit)
        {
            Playlists.Add(playList);
            return Response.Succeed();
        }
        return Response.Failed("You reach your playlist limit. upgrade your subscription");

    }

}
