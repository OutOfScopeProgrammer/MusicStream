using MusicStream.Domain.Entities;
using MusicStream.Infrastructure.Persistence.Postgres;

namespace MusicStream.Infrastructure.Repositories;

internal class RefreshTokenRepository(AppDbContext dbContext)
{



    public void AddRefreshToken(RefreshToken refreshToken)
        => dbContext.RefreshTokens.Add(refreshToken);
}
