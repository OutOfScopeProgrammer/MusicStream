using Microsoft.Extensions.Options;
using Minio;

namespace MusicStream.Infrastructure.Persistence.Minio;

internal class MinioConnection
{
    internal IMinioClient Client { get; set; }

    public MinioConnection(IOptions<MinioOption> options)
    {
        var config = options.Value;
        Client = new MinioClient()
        .WithEndpoint(config.Endpoint)
        .WithCredentials(config.UserName, config.Password)
        .WithSSL(config.WithSll)
        .Build();

    }



}
