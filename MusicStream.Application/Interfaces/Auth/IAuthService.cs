using MusicStream.Application.Common;

namespace MusicStream.Application.Interfaces.Auth;

public interface IAuthService
{
    Task<Response<AuthResult>> CreateUserWithFreeSubscription(string phoneNumber, string password, CancellationToken cancellationToken);
    Task<Response<AuthResult>> LoginWithUserNameAndPassword(string phoneNumber, string password, CancellationToken cancellationToken);
    Task<Response<AuthResult>> LoginUsingRefreshToken(string token, CancellationToken cancellationToken);
}
