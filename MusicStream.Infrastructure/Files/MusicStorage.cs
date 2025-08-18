using Minio;
using Minio.DataModel.Args;
using MusicStream.Application.Interfaces;
using MusicStream.Infrastructure.Persistence.Minio;

namespace MusicStream.Infrastructure.Files;

internal class MusicStorage(MinioConnection minio) : IMusicStorage
{
    private readonly IMinioClient Storage = minio.Client;
    private const string MUSICBUCKET = "music-bucket";



    public async Task UploadFile(string key, string filePath)
    {
        await Storage.PutObjectAsync(new PutObjectArgs()
        .WithBucket(MUSICBUCKET)
        .WithObject(key)
        .WithFileName(filePath)
        .WithContentType(""));
    }
    public async Task<MemoryStream> DownloadFile(string key)
    {
        var memory = new MemoryStream();
        var args = new GetObjectArgs()
        .WithBucket(MUSICBUCKET)
        .WithObject(key)
        .WithCallbackStream(stream =>
        {
            stream.CopyTo(memory);
        });
        await Storage.GetObjectAsync(args);
        memory.Position = 0;
        return memory;
    }

}
