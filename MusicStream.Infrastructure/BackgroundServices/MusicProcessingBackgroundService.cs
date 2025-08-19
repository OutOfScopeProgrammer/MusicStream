using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MusicStream.Application.Interfaces;
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
                var dto = await channel.ReadAsync();

                Console.WriteLine("Processing....");

                var outputFolder = Path.Combine(ROOTFOLDER, dto.FileName);
                Directory.CreateDirectory(outputFolder);

                await musicProcessor.ConvertForDash(dto.TempFilePath, outputFolder);

                var files = GetFiles(outputFolder);
                Console.WriteLine("Sending to minio....");

                await musicStorage.BatchUploadToMinio(files, ROOTFOLDER);

                await CleanUpDisk();
                //TODO: update music streamUrl
                var streamUrl = $"{dto.FileName}/manifest.mpd";
                await UpdateMusicStreamUrl(dto.musciEntityId, streamUrl);

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



    private async Task UpdateMusicStreamUrl(Guid musicId, string streamUrl)
    {
        using var scope = scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await dbContext.Musics.Where(m => m.Id == musicId)
        .ExecuteUpdateAsync(set =>
        set.SetProperty(m => m.StreamUrl, m => streamUrl));
    }


}
