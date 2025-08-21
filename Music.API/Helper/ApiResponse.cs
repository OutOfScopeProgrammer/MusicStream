namespace Music.API.Helper;

public record ApiResponse<T>(bool Success, int StatusCode,
T? Data, object? Errors, object? Meta)
{
    public static ApiResponse<T> Ok(T data)
     => new(true, StatusCodes.Status200OK, data, null, null);
    public static ApiResponse<T> Ok()
    => new(true, StatusCodes.Status200OK, default, null, null);
    public static ApiResponse<T> NotFound(string message)
   => new(false, StatusCodes.Status404NotFound, default, message, default);
    public static ApiResponse<T> BadRequest(object error)
    => new(false, StatusCodes.Status400BadRequest, default, error, default);
    public static ApiResponse<T> Created()
    => new(true, StatusCodes.Status201Created, default, default, default);

}

