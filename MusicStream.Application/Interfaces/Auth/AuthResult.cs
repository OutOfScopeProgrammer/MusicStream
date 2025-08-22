using MusicStream.Domain.Entities;

namespace MusicStream.Application.Interfaces.Auth;

public record class AuthResult(string AccessToken, RefreshToken RefreshToken);
