using MusicStream.Domain.Entities;

namespace MusicStream.Application.Interfaces.Repositories;

public interface IMusicRepository
{
    Task<Music?> GetMusicById(Guid musicId, bool asNoTracking, CancellationToken cancellationToken);
    Task<List<Music>?> GetMusics(CancellationToken cancellationToken);
    void AddMusic(Music music);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
