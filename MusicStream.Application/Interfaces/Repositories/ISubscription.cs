using MusicStream.Domain.Entities;

namespace MusicStream.Application.Interfaces.Repositories;

public interface ISubscription
{

    Task GetSubscriptionById(Guid subscriptionId, bool asNoTracking, CancellationToken cancellationToken);
    void AddSubscription(Subscription subscription);
    Task GetPlayistsByUserId(Guid userId, bool asNoTracking, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);


}
