namespace MusicStream.Application.Interfaces.Auth;

public record class AuthResult(string AccessToken, string RefreshToken);
