using Minio;
using Minio.DataModel.Args;
using MusicStream.Application.Interfaces;

namespace MusicStream.Infrastructure.Persistence.Minio;

internal class BucketManager(MinioConnection minio) : IBucketManager
{
    internal const string MAINBUCKET = "music-bucket";
    internal IMinioClient Minio { get; } = minio.Client;

    public async Task MainBucketInitializer()
    {
        var found = await Minio.BucketExistsAsync(new BucketExistsArgs().WithBucket(MAINBUCKET));
        if (!found)
            await Minio.MakeBucketAsync(new MakeBucketArgs().WithBucket(MAINBUCKET));
    }
}
