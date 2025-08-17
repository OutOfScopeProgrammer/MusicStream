using System.Text;
using Minio;
using Minio.DataModel.Args;
using MusicStream.Application.Interfaces;
using MusicStream.Infrastructure.Persistence.Minio;

namespace MusicStream.Infrastructure.Files;

internal class FileStorage(MinioConnection minio) : IFileStorage
{
    private readonly IMinioClient Storage = minio.Client;



    public async Task UploadFile(string bucket, string key, string filePath)
    {
        await Storage.PutObjectAsync(new PutObjectArgs()
        .WithBucket(bucket)
        .WithObject(key)
        .WithFileName(filePath)
        .WithContentType(""));
    }
    public async Task<string> DownloadFile(string bucket, string key)
    {
        using var memoryStream = new MemoryStream();
        var args = new GetObjectArgs()
        .WithBucket(bucket)
        .WithObject(key)
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
