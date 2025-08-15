using System.Text;
using Minio;
using Minio.DataModel.Args;
using MusicStream.Application.Interfaces;
using MusicStream.Infrastructure.Persistence.Minio;

namespace MusicStream.Infrastructure.Files;

internal class FileStorage(MinioConnection minio) : IFileStorage
{
    private readonly IMinioClient Storage = minio.Client;



    public async Task UploadFile(string bucket, string objectName, string content)
    {
        var data = Encoding.UTF8.GetBytes(content);
        var memoryStream = new MemoryStream(data);
        var args = new PutObjectArgs()
        .WithBucket(bucket)
        .WithObject(objectName)
        .WithStreamData(memoryStream)
        .WithObjectSize(memoryStream.Length)
        .WithContentType("text/plain");
        await Storage.PutObjectAsync(args);
    }
    public async Task<string> DownloadFile(string bucket, string objectName)
    {
        using var memoryStream = new MemoryStream();
        var args = new GetObjectArgs()
        .WithBucket(bucket)
        .WithObject(objectName)
        .WithCallbackStream(stream =>
        {
            stream.CopyTo(memoryStream);
        });
        await Storage.GetObjectAsync(args);
        memoryStream.Seek(0, SeekOrigin.Begin);
        using var reader = new StreamReader(memoryStream, Encoding.UTF8);
        return await reader.ReadToEndAsync();
    }
}
