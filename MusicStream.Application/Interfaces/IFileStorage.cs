namespace MusicStream.Application.Interfaces;

public interface IFileStorage
{
    Task UploadFile(string bucket, string objectName, string content);
    Task<string> DownloadFile(string bucket, string objectName);

}
