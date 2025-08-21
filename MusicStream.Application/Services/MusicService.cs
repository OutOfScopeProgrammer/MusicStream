using MusicStream.Application.Common;
using MusicStream.Application.Interfaces.Repositories;
using MusicStream.Domain.Entities;

namespace MusicStream.Application.Services;

public class MusicService(ISubscriptionRepository subRepository, IPlayListRepository playListRepository)
{
    // TODO: fix error messages
    public async Task<Response> CreatePlaylist(Guid userId, CancellationToken token)
    {
        var sub = await subRepository.GetSubscriptionByUserId(userId, false, token);
        if (sub is null)
            return Response.Failed(ErrorMessages.NotFound(nameof(sub)));

        var playList = Playlist.Create(sub);
        var msg = sub.TryAddPlaylist(playList);
        if (msg is not null)
            return Response.Failed(msg);

        await subRepository.SaveChangesAsync(token);
        return Response.Succeed();
    }
    public async Task<Response> DeletePlaylist(Guid userId, Guid playListId, CancellationToken token)
    {
        var playlist = await playListRepository.GetPlaylistById(playListId, false, token);
        if (playlist is null)
            return Response.Failed(ErrorMessages.NotFound(nameof(playlist)));
        playListRepository.DeletePlaylist(playlist);
        await playListRepository.SaveChangesAsync(token);
        return Response.Succeed();
    }
    public async Task UpdatePlaylist()
    {
        throw new NotImplementedException();

    }

    public async Task AddMusicToPlaylist()
    {
        throw new NotImplementedException();

    }
    public async Task DeleteMusicFromPlaylist()
    {
        throw new NotImplementedException();

    }


}
