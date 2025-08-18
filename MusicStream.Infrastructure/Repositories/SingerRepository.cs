using Microsoft.EntityFrameworkCore;
using MusicStream.Application.Interfaces.Repositories;
using MusicStream.Domain.Entities;
using MusicStream.Infrastructure.Persistence.Postgres;

namespace MusicStream.Infrastructure.Repositories;



internal class SingerRepository(AppDbContext dbContext) : ISingerRepository
{

    public async Task<Singer?> GetSingerById(Guid singerId, bool asNoTracking, CancellationToken cancellationToken)
    {
        var query = dbContext.Singers;
        if (asNoTracking)
            query.AsNoTracking();

        return await query.SingleOrDefaultAsync(s => s.Id == singerId, cancellationToken);
    }


    public async Task<Singer?> GetSingerByIdWithMusics(Guid singerId, bool asNoTracking, CancellationToken cancellationToken)
    {
        var query = dbContext.Singers;
        if (asNoTracking)
            query.AsNoTracking();
        return await query.Include(s => s.Musics)
        .SingleOrDefaultAsync(s => s.Id == singerId, cancellationToken);
    }

    public void AddSinger(Singer singer)
     => dbContext.Singers.Add(singer);


    public async Task SaveChangesAsync(CancellationToken cancellationToken)
        => await dbContext.SaveChangesAsync(cancellationToken);



}
