using MusicStream.Domain.Entities;

namespace MusicStream.Application.Interfaces.Repositories;

public interface IPlayListRepository
{
    Task<Playlist?> GetPlaylistById(Guid playlistId, bool asNoTracking, CancellationToken cancellationToken);
    Task<List<Playlist>> GetPlaylistsByUserId(Guid userId, bool asNoTracking, CancellationToken cancellationToken);
    void AddPlaylist(Playlist playlist);
    Task SaveChangesAsync(CancellationToken cancellationToken);

}
