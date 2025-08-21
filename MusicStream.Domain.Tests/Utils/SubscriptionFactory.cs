using MusicStream.Domain.Entities;

namespace MusicStream.Domain.Unit.Tests.Utils;

public class SubscriptionFactory
{

    public static List<Playlist> CreatePlayList(int n, Subscription sub)
    {
        var list = new List<Playlist>();
        for (var i = 0; i < n; i++)
        {
            var playlist = Playlist.Create(sub, "test");
            list.Add(playlist);
        }
        return list;
    }
}
