namespace MusicStream.Domain.Entities;

public class RefreshToken
{
    public RefreshToken() { }
    public Guid Id { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime ExpirationTime { get; set; }
    public User? User { get; set; }
    public Guid UserId { get; set; }

    public static RefreshToken Create(string token)
        => new()
        {
            Token = token,
            ExpirationTime = DateTime.UtcNow.AddDays(7)
        };
    public void SetTokenForUser(User user)
    {
        User = user;
        UserId = user.Id;
    }

    public bool IsValid()
    {
        if (ExpirationTime < DateTime.UtcNow)
            return false;

        return true;
    }
}
