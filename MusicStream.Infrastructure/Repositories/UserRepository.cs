using Microsoft.EntityFrameworkCore;
using MusicStream.Application.Interfaces.Repositories;
using MusicStream.Domain.Entities;
using MusicStream.Infrastructure.Persistence.Postgres;

namespace MusicStream.Infrastructure.Repositories;

internal class UserRepository(AppDbContext dbContext) : IUserRepository
{
    public Task<User?> GetUserById(Guid userId, bool asNoTracking, CancellationToken cancellationToken)
    {
        var query = dbContext.Users;
        if (asNoTracking)
            query.AsNoTracking();
        return query.SingleOrDefaultAsync(u => u.Id == userId, cancellationToken);
    }

    public void AddUser(User user)
    => dbContext.Users.Add(user);


    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    => await dbContext.SaveChangesAsync(cancellationToken);
}
