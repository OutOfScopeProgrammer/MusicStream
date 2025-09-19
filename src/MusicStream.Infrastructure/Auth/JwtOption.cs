namespace MusicStream.Infrastructure.Auth;

public record JwtOption
{
    public required string Issuer { get; set; }
    public required string Audience { get; set; }
    public required int ExpirationInMinutes { get; set; }
    public required string Secret { get; set; }


}
