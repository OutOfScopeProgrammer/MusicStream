using System;

namespace MusicStream.Application.Interfaces;

public interface IMusicChannel
{
    Task SendAsync(string item);
    Task ReadAsync();
    Task<bool> WaitToReadAsync();

}
