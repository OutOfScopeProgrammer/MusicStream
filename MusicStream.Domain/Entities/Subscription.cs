using System.Text.Json.Serialization;
using MusicStream.Domain.Common;

namespace MusicStream.Domain.Entities;

public class Subscription : Auditable
{
    public Subscription() { }
    public Guid Id { get; set; }
    public int PlaylistLimit { get; set; }
    [JsonIgnore]
    public List<Playlist> Playlists { get; set; } = [];
    public User User { get; set; } = new();
    public Guid UserId { get; set; }


    public static Subscription Create(int playListLimit, User user)
    => new()
    {
        PlaylistLimit = playListLimit,
        User = user
    };


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
