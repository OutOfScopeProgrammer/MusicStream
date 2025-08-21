using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MusicStream.Application.Interfaces;
using MusicStream.Domain.Entities;
using MusicStream.Infrastructure.Persistence.Postgres;
using MusicStream.Infrastructure.Processors;

namespace MusicStream.Infrastructure.BackgroundServices;

internal class MusicProcessingBackgroundService
(IMusicStorage musicStorage,
IMusicChannel channel,
 MusicProcessor musicProcessor,
 IServiceScopeFactory scopeFactory) : BackgroundService
{
    private const string ROOTFOLDER = @"E:\ASP.NET\MusicStream\Music.APi\wwwroot";
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {

            if (await channel.WaitToReadAsync())
            {
                var msg = await channel.ReadAsync();

                Console.WriteLine("Processing....");

                var outputFolder = Path.Combine(ROOTFOLDER, msg.FileName);
                Directory.CreateDirectory(outputFolder);

                await musicProcessor.ConvertForDash(msg.TempFilePath, outputFolder);

                var files = GetFiles(outputFolder);
                Console.WriteLine("Sending to minio....");

                await musicStorage.BatchUploadToMinio(files, ROOTFOLDER);

                await CleanUpDisk();
                var streamUrl = $"{msg.FileName}/manifest.mpd";
                await SaveMusic(msg.Title, msg.Description, streamUrl, msg.SingerId);

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



    private async Task SaveMusic(string title, string description, string streamUrl, Guid singerId)
    {
        using var scope = scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var music = Music.Create(title, description, false, singerId, streamUrl);
        dbContext.Add(music);
        await dbContext.SaveChangesAsync();
    }


}
