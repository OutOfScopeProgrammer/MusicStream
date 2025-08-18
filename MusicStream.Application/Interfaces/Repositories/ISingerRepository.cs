using MusicStream.Domain.Entities;

namespace MusicStream.Application.Interfaces.Repositories;

public interface ISingerRepository
{
    void AddSinger(Singer singer);
    Task<Singer?> GetSingerById(Guid singerId, bool asNoTracking, CancellationToken cancellationToken);
    Task<Singer?> GetSingerByIdWithMusics(Guid singerId, bool asNoTracking, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
