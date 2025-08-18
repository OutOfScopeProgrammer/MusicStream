namespace MusicStream.Infrastructure.Persistence.Minio;

internal record MinioOption
{
    public required string Endpoint { get; set; }
    public required string UserName { get; set; }
    public required string Password { get; set; }
    public required bool WithSsl { get; set; }
}
