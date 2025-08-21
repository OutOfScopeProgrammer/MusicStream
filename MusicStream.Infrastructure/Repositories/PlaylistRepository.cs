using Microsoft.EntityFrameworkCore;
using MusicStream.Application.Interfaces.Repositories;
using MusicStream.Domain.Entities;
using MusicStream.Infrastructure.Persistence.Postgres;

namespace MusicStream.Infrastructure.Repositories;

internal class PlaylistRepository(AppDbContext dbContext) : IPlayListRepository
{


    public async Task<Playlist?> GetPlaylistById(Guid playlistId, bool asNoTracking, CancellationToken cancellationToken)
    {
        var query = dbContext.Playlists.AsQueryable();
        if (asNoTracking)
            query = query.AsNoTracking();
        return await query.SingleOrDefaultAsync(p => p.Id == playlistId, cancellationToken);
    }
    public void AddPlaylist(Playlist playlist)
     => dbContext.Playlists.Add(playlist);



    public async Task<Playlist?> GetPlayListWithMusicsByPlaylistId(Guid playlistId, bool asNoTracking, CancellationToken cancellationToken)
    {
        var query = dbContext.Playlists.AsQueryable();
        if (asNoTracking)
            query = query.AsNoTracking();
        var musics = await query.Include(p => p.Musics).SingleOrDefaultAsync(p => p.Id == playlistId, cancellationToken);

        return musics;
    }
    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    => await dbContext.SaveChangesAsync(cancellationToken);

    public void DeletePlaylist(Playlist playlist)
        => dbContext.Playlists.Remove(playlist);


}
