namespace MusicStream.Application.Interfaces;

public record MusicChannelMessage(string TempFilePath, string RootFolder, string FileName,
 string Title, string Description, Guid SingerId);

public interface IMusicChannel
{
    Task SendAsync(MusicChannelMessage item);
    Task<MusicChannelMessage> ReadAsync();
    Task<bool> WaitToReadAsync();

}
