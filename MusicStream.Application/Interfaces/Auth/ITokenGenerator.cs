namespace MusicStream.Application.Interfaces.Auth;

public interface ITokenGenerator
{
    string JwtToken(Guid id);
    string RefreshToken();
}
