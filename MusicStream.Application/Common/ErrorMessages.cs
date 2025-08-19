namespace MusicStream.Application.Common;

public static class ErrorMessages
{
    public static string NotFound(object? obj)
    {
        return $"{nameof(obj)} is not found";
    }
}
