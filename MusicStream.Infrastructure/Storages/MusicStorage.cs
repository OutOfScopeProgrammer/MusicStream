using Minio;
using Minio.DataModel.Args;
using MusicStream.Application.Interfaces;
using MusicStream.Infrastructure.Persistence.Minio;

namespace MusicStream.Infrastructure.Storages;

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
    public async Task BatchUploadToMinio(IEnumerable<string> files, string rootFolder)
    {
        var concurencyLimit = new SemaphoreSlim(4);
        var tasks = new List<Task>();
        foreach (var file in files)
        {
            await concurencyLimit.WaitAsync();
            var relativePath = Path.GetRelativePath(rootFolder, file);
            var key = relativePath.Replace("\\", "/");
            var task = Task.Run(async () =>
            {
                try
                {
                    {
                        using var stream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.None);
                        await Storage.PutObjectAsync(new PutObjectArgs()
                            .WithBucket("music-bucket")
                            .WithObject(key)
                            .WithStreamData(stream)
                            .WithContentType(""));
                    }
                }
                finally
                {
                    concurencyLimit.Release();
                }
            });
            tasks.Add(task);
        }
        await Task.WhenAll(tasks);

    }

}