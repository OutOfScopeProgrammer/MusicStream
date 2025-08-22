using Microsoft.EntityFrameworkCore;
using MusicStream.Domain.Entities;
using MusicStream.Infrastructure.Persistence.Postgres;

namespace MusicStream.Infrastructure.Repositories;

internal class RefreshTokenRepository(AppDbContext dbContext)
{



    public void AddRefreshToken(RefreshToken refreshToken)
        => dbContext.RefreshTokens.Add(refreshToken);

    public async Task<RefreshToken?> GetRefreshTokenByToken(string token)
    => await dbContext.RefreshTokens.Include(r => r.User)
        .ThenInclude(u => u!.Subscription)
                .SingleOrDefaultAsync(r => r.Token == token);




    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    => await dbContext.SaveChangesAsync(cancellationToken);

}
