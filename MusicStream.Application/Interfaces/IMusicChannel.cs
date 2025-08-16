namespace MusicStream.Application.Interfaces;

public interface IMusicChannel
{
    Task SendAsync(ChannelDto item);
    Task<ChannelDto> ReadAsync();
    Task<bool> WaitToReadAsync();

}
