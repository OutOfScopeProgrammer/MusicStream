using MusicStream.Application.Common;
using MusicStream.Application.Interfaces.Repositories;
using MusicStream.Application.Services;
using MusicStream.Domain.Entities;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace MusicStream.Application.Tests.PlaylistServiceTests;

public class GetPlaylistWithMusicsTests
{
    private readonly ISubscriptionRepository subscriptionRepository = Substitute.For<ISubscriptionRepository>();
    private readonly IMusicRepository musicRepository = Substitute.For<IMusicRepository>();
    private readonly IPlayListRepository playlistRepository = Substitute.For<IPlayListRepository>();
    private PlaylistService _sut;
    public GetPlaylistWithMusicsTests()
    {

        _sut = new PlaylistService(subscriptionRepository, playlistRepository,
         musicRepository);

    }
    [Fact]
    public async Task PlaylistNull_ReturnsFailed()
    {
        // Arrange
        playlistRepository.GetPlaylistWithMusicsByPlaylistId(Guid.NewGuid(), false, CancellationToken.None).ReturnsNull();
        // Act
        var result = await _sut.GetPlaylistWithMusics(Guid.NewGuid(), CancellationToken.None);
        // Assert
        Assert.False(result.IsSuccess);
        Assert.Null(result.Data);
        var expectedError = ErrorMessages.NotFound(nameof(result.Data));
        Assert.Contains(expectedError, result.Error);
    }
    [Fact]
    public async Task PlaylistExist_ReturnsTrue()
    {
        // Arrange
        var pl = Playlist.Create(new Subscription(), "test");
        var id = Guid.NewGuid();
        playlistRepository.GetPlaylistWithMusicsByPlaylistId(id, false, CancellationToken.None)
        .Returns(pl);
        // Act
        var result = await _sut.GetPlaylistWithMusics(id, CancellationToken.None);
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(pl.Title, result.Data.Title);
        Assert.Empty(result.Error);
    }
}
