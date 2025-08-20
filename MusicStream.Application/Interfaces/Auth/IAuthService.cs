using MusicStream.Application.Common;

namespace MusicStream.Application.Interfaces.Auth;

public interface IAuthService
{
    Task<Response<AuthResult>> CreateUserWithFreeSubscription(string phoneNumber, string password);
    Task<Response<AuthResult>> Login(string phoneNumber, string password);
}
