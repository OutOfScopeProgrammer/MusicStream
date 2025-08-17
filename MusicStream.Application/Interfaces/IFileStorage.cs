namespace MusicStream.Application.Interfaces;

public interface IFileStorage
{
    Task UploadFile(string bucket, string objectName, string content);
    Task<MemoryStream> DownloadFile(string bucket, string objectName);

}
