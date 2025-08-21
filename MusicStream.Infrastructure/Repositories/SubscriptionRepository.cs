using Microsoft.EntityFrameworkCore;
using MusicStream.Application.Interfaces.Repositories;
using MusicStream.Domain.Entities;
using MusicStream.Infrastructure.Persistence.Postgres;

namespace MusicStream.Infrastructure.Repositories;

internal class SubscriptionRepository(AppDbContext dbContext) : ISubscriptionRepository
{


    public async Task<Subscription?> GetSubscriptionByUserId(Guid userId, bool asNoTracking, CancellationToken cancellationToken)
    {
        var query = dbContext.Subscriptions.AsQueryable();
        if (asNoTracking)
            query = query.AsNoTracking();
        var sub = await query.Include(s => s.Playlists).SingleOrDefaultAsync(s => s.UserId == userId);
        return sub;
    }
    public void AddSubscription(Subscription subscription)
        => dbContext.Subscriptions.Add(subscription);
    public async Task<Subscription?> GetSubscriptionById(Guid subscriptionId, bool asNoTracking, CancellationToken cancellationToken)
    {
        var query = dbContext.Subscriptions.AsQueryable();
        if (asNoTracking)
            query = query.AsNoTracking();
        return await query.SingleOrDefaultAsync(s => s.Id == subscriptionId, cancellationToken);
    }
    public async Task SaveChangesAsync(CancellationToken cancellationToken)
        => await dbContext.SaveChangesAsync(cancellationToken);
}
