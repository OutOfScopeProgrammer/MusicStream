using MusicStream.Domain.Entities;

namespace MusicStream.Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetUserById(Guid userId, bool asNoTracking, CancellationToken cancellationToken);
    Task<User?> GetUserByPhoneNumber(string phoneNumber, bool asNoTracking, CancellationToken cancellationToken)
    void AddUser(User user);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
