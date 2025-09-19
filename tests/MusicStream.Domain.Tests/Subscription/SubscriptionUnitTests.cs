using MusicStream.Domain.Entities;
using MusicStream.Domain.Enums;

public class SubscriptionUnitTests
{
    [Theory]
    [InlineData(SubscriptionType.Free)]
    [InlineData(SubscriptionType.Basic)]
    [InlineData(SubscriptionType.Premium)]
    public void AddPlayListMoreThanLimit_ReturnsFailed(SubscriptionType type)
    {
        // Arrange
        var sub = Subscription.Create(type);
        var list = new List<Playlist>();
        for (int i = 0; i < sub.PlaylistLimit; i++)
        {
            var pl = Playlist.Create(sub, "test");
            list.Add(pl);
        }
        sub.Playlists = list;
        // Act
        var playList = Playlist.Create(sub, "more than allowed");
        var result = sub.TryAddPlaylist(playList);
        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("You reach", result.Error);

    }
    [Theory]
    [InlineData(SubscriptionType.Basic)]
    [InlineData(SubscriptionType.Premium)]
    public void AddPlayListLessThanLimit_ReturnsSucess(SubscriptionType type)
    {
        // Arrange
        var sub = Subscription.Create(type);
        var list = new List<Playlist>();
        for (int i = 0; i < sub.PlaylistLimit - 1; i++)
        {
            var pl = Playlist.Create(sub, "test");
            list.Add(pl);
        }
        sub.Playlists = list;
        // Act
        var playList = Playlist.Create(sub, "more than allowed");
        var result = sub.TryAddPlaylist(playList);
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Error);
    }

}
