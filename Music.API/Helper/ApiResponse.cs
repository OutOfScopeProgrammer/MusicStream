namespace Music.API.Helper;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public int StatusCode { get; set; }
    public T? Data { get; set; }
    public object? Errors { get; set; }
    public object? Meta { get; set; }

}
