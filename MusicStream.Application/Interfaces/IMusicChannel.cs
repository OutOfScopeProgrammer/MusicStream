namespace MusicStream.Application.Interfaces;

public record MusicChannelMessage(string TempFilePath, string RootFolder, string StoredName);

public interface IMusicChannel
{
    Task SendAsync(MusicChannelMessage item);
    Task<MusicChannelMessage> ReadAsync();
    Task<bool> WaitToReadAsync();

}
