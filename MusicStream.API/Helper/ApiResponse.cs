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
    public static ApiResponse<T> NoContent()
    => new(true, StatusCodes.Status204NoContent, default, default, default);

}
public record ApiResponse(bool Success, int StatusCode,
 object? Errors, object? Meta)
{
    public static ApiResponse Ok()
    => new(true, StatusCodes.Status200OK, null, null);
    public static ApiResponse NotFound(string message)
   => new(false, StatusCodes.Status404NotFound, message, default);
    public static ApiResponse BadRequest(object error)
    => new(false, StatusCodes.Status400BadRequest, error, default);
    public static ApiResponse Created()
    => new(true, StatusCodes.Status201Created, default, default);
    public static ApiResponse NoContent()
   => new(true, StatusCodes.Status204NoContent, default, default);

}

