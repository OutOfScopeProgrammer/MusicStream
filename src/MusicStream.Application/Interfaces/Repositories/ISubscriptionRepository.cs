using MusicStream.Domain.Entities;

namespace MusicStream.Application.Interfaces.Repositories;

public interface ISubscriptionRepository
{

    Task<Subscription?> GetSubscriptionById(Guid subscriptionId, bool asNoTracking, CancellationToken cancellationToken);
    void AddSubscription(Subscription subscription);
    Task<Subscription?> GetSubscriptionByUserId(Guid userId, bool asNoTracking, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);


}
