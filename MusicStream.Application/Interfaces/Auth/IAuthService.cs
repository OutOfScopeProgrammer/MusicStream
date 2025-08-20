using MusicStream.Application.Common;

namespace MusicStream.Application.Interfaces.Auth;

public interface IAuthService
{
    Task<Response<AuthResult>> CreateUserWithFreeSubscriptionType(string phoneNumber, string password);
    Task<Response<AuthResult>> Login(string phoneNumber, string password);
}
