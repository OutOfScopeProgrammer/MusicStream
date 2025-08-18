using Microsoft.EntityFrameworkCore;
using MusicStream.Application.Interfaces.Repositories;
using MusicStream.Domain.Entities;
using MusicStream.Infrastructure.Persistence.Postgres;

namespace MusicStream.Infrastructure.Repositories;

internal class PlaylistRepository(AppDbContext dbContext) : IPlayListRepository
{


    public async Task<Playlist?> GetPlaylistById(Guid playlistId, bool asNoTracking, CancellationToken cancellationToken)
    {
        var query = dbContext.Playlists;
        if (asNoTracking)
            query.AsNoTracking();
        return await query.SingleOrDefaultAsync(p => p.Id == playlistId, cancellationToken);
    }
    public void AddPlaylist(Playlist playlist)
     => dbContext.Playlists.Add(playlist);
    public Task<List<Playlist>> GetPlaylistsByUserId(Guid userId, bool asNoTracking, CancellationToken cancellationToken)
    {
        var query = dbContext.Playlists;
        if (asNoTracking)
            query.AsNoTracking();

        var playlists = query.Include(p => p.Subscription)
            .Where(p => p.Subscription.UserId == userId).ToListAsync(cancellationToken);
        return playlists;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    => await dbContext.SaveChangesAsync(cancellationToken);

}
