using Microsoft.AspNetCore.Identity;
using MusicStream.Application.Common;
using MusicStream.Application.Interfaces.Auth;
using MusicStream.Application.Interfaces.Repositories;
using MusicStream.Domain.Entities;

namespace MusicStream.Infrastructure.Auth;

internal class AuthService
(IUserRepository userRepository, ITokenGenerator tokenGenerator
, IPasswordHasher<User> passwordHasher) : IAuthService
{


    public async Task<Response<AuthResult>> CreateUserWithFreeSubscription(string phoneNumber, string password)
    {
        var exist = await userRepository.GetUserByPhoneNumber(phoneNumber, true, CancellationToken.None) is not null;
        if (exist)
            return Response<AuthResult>.Failed(ErrorMessages.DuplicatePhoneNumber());

        var user = User.Create(phoneNumber);
        var subscription = Subscription.Create(Domain.Enums.SubscriptionType.Free);
        user.SetSubscription(subscription);
        var hashedPassword = passwordHasher.HashPassword(user, password);
        user.SetHashedPassword(hashedPassword);

        userRepository.AddUser(user);
        await userRepository.SaveChangesAsync(CancellationToken.None);

        var accessToken = tokenGenerator.JwtToken(user.Id);
        return Response<AuthResult>.Succeed(new AuthResult(accessToken, ""));

    }

    public Task<Response<AuthResult>> Login(string phoneNumber, string password)
    {
        throw new NotImplementedException();
    }
}
