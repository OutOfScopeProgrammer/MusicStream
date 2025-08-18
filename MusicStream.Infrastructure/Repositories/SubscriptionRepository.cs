using Microsoft.EntityFrameworkCore;
using MusicStream.Application.Interfaces.Repositories;
using MusicStream.Domain.Entities;
using MusicStream.Infrastructure.Persistence.Postgres;

namespace MusicStream.Infrastructure.Repositories;

internal class SubscriptionRepository(AppDbContext dbContext) : ISubscriptionRepository
{


    public async Task<List<Playlist>> GetPlayistsByUserId(Guid userId, bool asNoTracking, CancellationToken cancellationToken)
    {
        var query = dbContext.Playlists;
        if (asNoTracking)
            query.AsNoTracking();
        var playlists = await query.Include(p => p.Subscription)
        .Where(p => p.Subscription.UserId == userId).ToListAsync(cancellationToken);
        return playlists;
    }
    public void AddSubscription(Subscription subscription)
        => dbContext.Subscriptions.Add(subscription);
    public async Task<Subscription?> GetSubscriptionById(Guid subscriptionId, bool asNoTracking, CancellationToken cancellationToken)
    {
        var query = dbContext.Subscriptions;
        if (asNoTracking)
            query.AsNoTracking();
        return await query.SingleOrDefaultAsync(s => s.Id == subscriptionId, cancellationToken);
    }
    public async Task SaveChangesAsync(CancellationToken cancellationToken)
        => await dbContext.SaveChangesAsync(cancellationToken);
}
