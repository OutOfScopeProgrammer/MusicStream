using MusicStream.Domain.Entities;
using MusicStream.Domain.Enums;

namespace MusicStream.Domain.Tests.Playlist;

public class PlaylistTests
{

    [Theory]
    [InlineData(SubscriptionType.Basic)]
    [InlineData(SubscriptionType.Premium)]
    public void AddMusicMoreThanLimit_ReturnsFailed(SubscriptionType type)
    {
        // Arrange
        var sub = Subscription.Create(type);
        var pl = Entities.Playlist.Create(sub, "test");
        for (int i = 0; i < pl.MusicLimits; i++)
        {
            var music = Music.Create("test", "test", "test", "test", "test", true, "test");
            pl.TryAddMusic(music);
        }
        // Act

        var msc = Music.Create("test", "test", "test", "test", "test", true, "test");
        var result = pl.TryAddMusic(msc);
        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("You're play", result.Error);
    }
}
