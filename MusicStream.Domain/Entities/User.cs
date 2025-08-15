using MusicStream.Domain.Common;

namespace MusicStream.Domain.Entities;

public class User : Auditable
{
    public User() { }
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string HashedPassword { get; set; } = string.Empty;

    public Subscription Subscription { get; set; } = new();

    public void SetSubscription(Subscription subscription)
    {
        Subscription = subscription;
    }

    public static User Create(string userName) => new() { Username = userName };

    public void SetHashedPassword(string hashedPassword) => HashedPassword = hashedPassword;
}
