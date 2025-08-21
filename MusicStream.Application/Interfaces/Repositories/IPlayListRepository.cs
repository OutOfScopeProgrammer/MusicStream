using MusicStream.Domain.Entities;

namespace MusicStream.Application.Interfaces.Repositories;

public interface IPlayListRepository
{
    Task<Playlist?> GetPlaylistById(Guid playlistId, bool asNoTracking, CancellationToken cancellationToken);
    void AddPlaylist(Playlist playlist);
    Task<Playlist?> GetPlaylistWithMusicsByPlaylistId(Guid playlistId, bool asNoTracking, CancellationToken cancellationToken);
    void DeletePlaylist(Playlist playlist);
    Task SaveChangesAsync(CancellationToken cancellationToken);

}
