using Microsoft.EntityFrameworkCore;
using MusicStream.Application.Interfaces.Repositories;
using MusicStream.Domain.Entities;
using MusicStream.Infrastructure.Persistence.Postgres;

namespace MusicStream.Infrastructure.Repositories;

internal class MusicRepository(AppDbContext dbContext) : IMusicRepository
{

    public async Task<Music?> GetMusicById(Guid musicId, bool asNoTracking, CancellationToken cancellationToken)
    {
        var query = dbContext.Musics;
        if (asNoTracking)
            query.AsNoTracking();
        return await query.SingleOrDefaultAsync(m => m.Id == musicId, cancellationToken);
    }

    public async Task<List<Music>?> GetMusics(CancellationToken cancellationToken)
    {
        return await dbContext.Musics.ToListAsync(cancellationToken);
    }
    public void AddMusic(Music music)
    => dbContext.Add(music);


    public async Task SaveChangesAsync(CancellationToken cancellationToken)
        => await dbContext.SaveChangesAsync(cancellationToken);
}
