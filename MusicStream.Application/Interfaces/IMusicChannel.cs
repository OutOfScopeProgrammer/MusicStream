namespace MusicStream.Application.Interfaces;

public interface IMusicChannel
{
    Task SendAsync(MusicChannelMessage item);
    Task<MusicChannelMessage> ReadAsync();
    Task<bool> WaitToReadAsync();

}
