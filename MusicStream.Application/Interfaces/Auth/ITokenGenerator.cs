namespace MusicStream.Application.Interfaces.Auth;

public interface ITokenGenerator
{
    string JwtToken(Guid user, Guid subscriptionId);
    string RefreshToken();
}
