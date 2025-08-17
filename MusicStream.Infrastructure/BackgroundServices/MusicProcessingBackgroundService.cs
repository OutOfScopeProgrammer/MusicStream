using System.Security.AccessControl;
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
            Console.WriteLine("Processing....");

            if (await channel.WaitToReadAsync())
            {
                var dto = await channel.ReadAsync();
                var accFile = await musicProcessor.ConvertToAcc(dto.TempFilePath, dto.RootFolder);
                await musicProcessor.ConvertToHls(accFile, dto.RootFolder, dto.FileName);
                var files = GetFiles(Path.Combine(dto.RootFolder, dto.FileName));
                await BatchUploadToMinio(files);
                await CleanUpDisk();
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

    private List<string> GetFiles(string dirPath)
    {
        var list = new List<string>();
        var dirs = Directory.GetDirectories(dirPath);
        foreach (var dir in dirs)
        {
            list.AddRange(Directory.GetFiles(dir));
        }
        return list;

    }

    private async Task BatchUploadToMinio(List<string> files)
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
