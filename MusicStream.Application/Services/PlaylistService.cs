using MusicStream.Application.Common;
using MusicStream.Application.Interfaces.Repositories;
using MusicStream.Domain.Entities;

namespace MusicStream.Application.Services;

public class PlaylistService(ISubscriptionRepository subRepository, IPlayListRepository playListRepository,
IMusicRepository musicRepository)
{
    // TODO: fix error messages
    public async Task<Response<Playlist>> GetPlaylistWithMusics(Guid playlistId, CancellationToken cancellationToken)
    {
        var playlist = await playListRepository.GetPlaylistWithMusicsByPlaylistId(playlistId, false, cancellationToken);
        if (playlist is null)
            return Response<Playlist>.Failed(ErrorMessages.NotFound(nameof(playlist)));

        return Response<Playlist>.Succeed(playlist);
    }
    public async Task<Response> CreatePlaylist(Guid userId, CancellationToken token, string title)
    {
        var sub = await subRepository.GetSubscriptionByUserId(userId, false, token);
        if (sub is null)
            return Response.Failed(ErrorMessages.NotFound(nameof(sub)));

        var playList = Playlist.Create(sub, title);
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
    public async Task<Response> UpdatePlaylist(string title, Guid playlistId, CancellationToken token)
    {
        var playList = await playListRepository.GetPlaylistById(playlistId, false, token);
        if (playList is null)
            return Response.Failed(ErrorMessages.NotFound(nameof(playList)));

        playList.UpdateTitle(title);
        await playListRepository.SaveChangesAsync(token);
        return Response.Succeed();
    }

    public async Task<Response> AddMusicToPlaylist(Guid musicId, Guid playlistId, CancellationToken token)
    {
        var playList = await playListRepository.GetPlaylistById(playlistId, false, token);
        if (playList is null)
            return Response.Failed(ErrorMessages.NotFound(nameof(playList)));
        var music = await musicRepository.GetMusicById(musicId, false, token);
        if (music is null)
            return Response.Failed(ErrorMessages.NotFound(nameof(music)));

        playList.AddMusic(music);
        await playListRepository.SaveChangesAsync(token);

        return Response.Succeed();
    }
    public async Task<Response> RemoveMusicFromPlaylist(Guid musicId, Guid playlistId, CancellationToken token)
    {
        var playlist = await playListRepository.GetPlaylistWithMusicsByPlaylistId(playlistId, false, token);
        if (playlist is null)
            return Response.Failed(ErrorMessages.NotFound(nameof(playlist)));

        var music = playlist.Musics.FirstOrDefault(m => m.Id == musicId);
        if (music is null)
            return Response.Failed(ErrorMessages.NotFound(nameof(music)));

        playlist.RemoveMusic(music);
        await playListRepository.SaveChangesAsync(token);

        return Response.Succeed();
    }


}
