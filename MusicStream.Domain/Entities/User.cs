using System.Text.Json.Serialization;
using MusicStream.Domain.Common;

namespace MusicStream.Domain.Entities;

public class User : Auditable
{
    public User() { }
    public Guid Id { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string HashedPassword { get; set; } = string.Empty;

    [JsonIgnore]
    public Subscription? Subscription { get; set; }
    public Guid SubscriptionId { get; set; }
    public RefreshToken? RefreshToken { get; set; }

    public void SetSubscription(Subscription subscription)
    {
        Subscription = subscription;
    }

    public static User Create(string phoneNumber) => new()
    {
        PhoneNumber = phoneNumber,

    };

    public void SetHashedPassword(string hashedPassword) => HashedPassword = hashedPassword;
}
