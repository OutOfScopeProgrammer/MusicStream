using MusicStream.Domain.Entities;

namespace MusicStream.Application.Interfaces.Auth;

public interface ITokenGenerator
{
    string JwtToken(User user);
    string RefreshToken();
}
