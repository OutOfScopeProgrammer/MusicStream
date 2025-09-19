namespace MusicStream.Application.Interfaces;

public interface IMusicStorage
{
    Task UploadFile(string objectName, string content);
    Task<MemoryStream> DownloadFile(string objectName);
    Task BatchUploadToMinio(IEnumerable<string> files, string rootFolder);

}
