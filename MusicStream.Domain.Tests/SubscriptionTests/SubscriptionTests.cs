using MusicStream.Domain.Entities;
using MusicStream.Domain.Enums;
using MusicStream.Domain.Unit.Tests.Utils;

namespace MusicStream.Domain.Unit.Tests.SubscriptionTests;

public class SubscriptionTests
{

    [Theory]
    [InlineData(0, SubscriptionType.Free)]
    [InlineData(2, SubscriptionType.Basic)]
    [InlineData(10, SubscriptionType.Premium)]

    public void AddPlayList_When_PlayListAreMoreThanSubscriptionLimit_ShouldReturnFalse(int limit, SubscriptionType type)
    {
        // Given
        var sub = Subscription.Create(type);
        var list = SubscriptionFactory.CreatePlayList(limit, sub);
        sub.Playlists = list;
        // When
        var playList = Playlist.Create(sub, "test");
        var result = sub.TryAddPlaylist(playList);
        // Then
        Assert.False(result.IsSuccess);
    }
    [Theory]
    [InlineData(2, SubscriptionType.Basic)]
    [InlineData(10, SubscriptionType.Premium)]

    public void AddPlayList_When_PlayListAreMoreThanSubscriptionLimit_ShouldReturnTrue(int limit, SubscriptionType type)
    {
        // Given
        var sub = Subscription.Create(type);
        var list = SubscriptionFactory.CreatePlayList(limit - 1, sub);
        sub.Playlists = list;
        // When
        var playList = Playlist.Create(sub, "test");
        var result = sub.TryAddPlaylist(playList);
        // Then
        Assert.True(result.IsSuccess);
    }

}