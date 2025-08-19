namespace MusicStream.Application.Interfaces;

public record MusicChannelMessage(string TempFilePath, string RootFolder, string FileName, Guid musciEntityId);

public interface IMusicChannel
{
    Task SendAsync(MusicChannelMessage item);
    Task<MusicChannelMessage> ReadAsync();
    Task<bool> WaitToReadAsync();

}
