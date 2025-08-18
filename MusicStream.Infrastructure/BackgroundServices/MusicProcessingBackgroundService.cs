using Microsoft.Extensions.Hosting;
using Minio;
using Minio.DataModel.Args;
using MusicStream.Application.Interfaces;
using MusicStream.Infrastructure.Persistence.Minio;
using MusicStream.Infrastructure.Processors;

namespace MusicStream.Infrastructure.BackgroundServices;

internal class MusicProcessingBackgroundService(MinioConnection minio, IMusicChannel channel, MusicProcessor musicProcessor) : BackgroundService
{
    private readonly IMinioClient Storage = minio.Client;
    private const string ROOTFOLDER = @"E:\ASP.NET\MusicStream\Music.APi\wwwroot";
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {

            if (await channel.WaitToReadAsync())
            {
                var dto = await channel.ReadAsync();
                Console.WriteLine("Processing....");
                var outputFolder = Path.Combine(ROOTFOLDER, dto.FileName);
                Directory.CreateDirectory(outputFolder);
                await musicProcessor.ConvertForDash(dto.TempFilePath, outputFolder);
                var files = GetFiles(outputFolder);
                Console.WriteLine("Sending to minio....");
                await BatchUploadToMinio(files);
                await CleanUpDisk();
                var streamUrl = $"{dto.FileName}/manifest.mpd";
            }

        }
    }



    private async Task CleanUpDisk()
    {
        foreach (var dir in Directory.GetDirectories(ROOTFOLDER))
        {
            await Task.Run(() => Directory.Delete(dir, true));
        }
    }

    private string[] GetFiles(string dirPath)
    {
        var dirs = Directory.GetFiles(dirPath);

        return dirs;

    }

    private async Task BatchUploadToMinio(string[] files)
    {
        var tasks = new List<Task>();
        foreach (var file in files)
        {
            tasks.Add(Task.Run(async () =>
            {
                var relativePath = Path.GetRelativePath(ROOTFOLDER, file);
                var key = relativePath.Replace("\\", "/");
                await Storage.PutObjectAsync(new PutObjectArgs()
        .WithBucket("music-bucket")
        .WithObject(key)
        .WithFileName(file)
        .WithContentType(""));
            }));
        }

        await Task.WhenAll(tasks);
    }
}
