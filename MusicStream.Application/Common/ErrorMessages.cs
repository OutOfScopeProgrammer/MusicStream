namespace MusicStream.Application.Common;

public static class ErrorMessages
{
    public static string NotFound(object? obj)
    {
        return $"{nameof(obj)} is not found.";
    }
    public static string DuplicatePhoneNumber()
    {
        return $"phoneNumber is duplicated.";
    }
    public static string LoginError()
    => $"username or password is wrong.";
    public static string RefreshTokenError()
    => $"invalid token.";

}
