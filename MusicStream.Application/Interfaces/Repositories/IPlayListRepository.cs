using MusicStream.Domain.Entities;

namespace MusicStream.Application.Interfaces.Repositories;

public interface IPlayListRepository
{
    Task<Playlist?> GetPlaylistById(Guid playlistId, bool asNoTracking, CancellationToken cancellationToken);
    void AddPlaylist(Playlist playlist);
    Task<List<Music>> GetMusicsByPlaylistId(Guid playlistId, bool asNoTracking, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);

}
