using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using MusicStream.Application.Common;
using MusicStream.Application.Interfaces.Auth;
using MusicStream.Application.Interfaces.Repositories;
using MusicStream.Domain.Entities;
using MusicStream.Infrastructure.Repositories;

namespace MusicStream.Infrastructure.Auth;

internal class AuthService
(IUserRepository userRepository, ITokenGenerator tokenGenerator
, IPasswordHasher<User> passwordHasher, RefreshTokenRepository tokenRepository) : IAuthService
{


    public async Task<Response<AuthResult>> CreateUserWithFreeSubscription(string phoneNumber, string password, CancellationToken cancellationToken)
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
        var token = tokenGenerator.RefreshToken();
        var refreshToken = RefreshToken.Create(token);
        refreshToken.SetTokenForUser(user);
        tokenRepository.AddRefreshToken(refreshToken);
        await userRepository.SaveChangesAsync(cancellationToken);

        var accessToken = tokenGenerator.JwtToken(user.Id);
        return Response<AuthResult>.Succeed(new AuthResult(accessToken, refreshToken.Token));

    }

    public async Task<Response<AuthResult>> Login(string phoneNumber, string password, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUserByPhoneNumber(phoneNumber, false, cancellationToken);
        if (user is null)
            return Response<AuthResult>.Failed(ErrorMessages.LoginError());
        var isCorrect = passwordHasher.VerifyHashedPassword(user, user.HashedPassword, password);
        if (isCorrect == PasswordVerificationResult.Failed)
            return Response<AuthResult>.Failed(ErrorMessages.LoginError());

        var newRefreshToken = tokenGenerator.RefreshToken();
        await tokenRepository.UpdateUserRefreshTokenByUserId(user.Id, newRefreshToken);
        var accessToken = tokenGenerator.JwtToken(user.Id);
        return Response<AuthResult>.Succeed(new AuthResult(accessToken, newRefreshToken));
    }


    public async Task<Response<AuthResult>> LoginUsingRefreshToken(string token, CancellationToken cancellationToken)
    {
        var refreshToken = await tokenRepository.GetRefreshTokenByToken(token);
        if (refreshToken is null)
            return Response<AuthResult>.Failed(ErrorMessages.RefreshTokenError());
        var newRefreshToken = tokenGenerator.RefreshToken();
        refreshToken.Token = newRefreshToken;

        await tokenRepository.SaveChangesAsync(cancellationToken);

        var accessToken = tokenGenerator.JwtToken(refreshToken.UserId);
        return Response<AuthResult>.Succeed(new AuthResult(accessToken, newRefreshToken));

    }


}
