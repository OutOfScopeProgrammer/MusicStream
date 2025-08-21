using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using MusicStream.Domain.Entities;
using MusicStream.Infrastructure.Persistence.Postgres;

namespace MusicStream.Infrastructure.Repositories;

internal class RefreshTokenRepository(AppDbContext dbContext)
{



    public void AddRefreshToken(RefreshToken refreshToken)
        => dbContext.RefreshTokens.Add(refreshToken);

    public async Task<RefreshToken?> GetRefreshTokenByToken(string token)
    => await dbContext.RefreshTokens.Include(r => r.User)
                .SingleOrDefaultAsync(r => r.Token == token);


    public async Task<bool> UpdateUserRefreshTokenByUserId(Guid userId, string token)
    {
        return await dbContext.RefreshTokens
        .Where(r => r.UserId == userId)
        .ExecuteUpdateAsync
        (r => r.SetProperty(r => r.Token, m => token)) > 0;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    => await dbContext.SaveChangesAsync(cancellationToken);

}
