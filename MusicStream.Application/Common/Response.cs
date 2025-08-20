using System.Diagnostics.CodeAnalysis;

namespace MusicStream.Application.Common;

public class Response
{
    [MemberNotNullWhen(false, nameof(Error))]
    public bool IsSuccess { get; set; }
    public string? Error { get; set; } = string.Empty;
    private Response(bool isSucess, string? errors)
    {
        IsSuccess = isSucess;
        Error = errors ?? Error;
    }

    public static Response Succeed() => new(true, null);
    public static Response Failed(string error) => new(false, error);

}
public class Response<T>
{
    [MemberNotNullWhen(true, nameof(Data))]
    public bool IsSuccess { get; set; }
    public T? Data { get; set; }
    public string Error { get; set; } = string.Empty;
    private Response(T? data, bool isSucess, string? error)
    {
        Data = data;
        IsSuccess = isSucess;
        Error = error ?? Error;
    }
    private Response(bool isSucess, string error)
    {
        IsSuccess = isSucess;
        Error = error ?? Error;
    }

    public static Response<T> Succeed(T data) => new(data, true, null);
    public static Response<T> Failed(string error) => new(false, error);

}
