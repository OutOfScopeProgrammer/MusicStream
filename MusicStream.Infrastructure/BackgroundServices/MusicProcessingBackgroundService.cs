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

                var metaData = await musicProcessor.ConvertForDash(msg.TempFilePath, outputFolder);

                var files = GetFiles(outputFolder);
                Console.WriteLine("Sending to minio....");

                await musicStorage.BatchUploadToMinio(files, ROOTFOLDER);

                await CleanUpDisk();
                var streamUrl = $"{msg.FileName}/manifest.mpd";
                // await SaveMusic(msg.Title, msg.Description, streamUrl);

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



    private async Task SaveMusic(string title, string description, string streamUrl)
    {
        using var scope = scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var music = Music.Create(title, description, false, streamUrl);
        dbContext.Add(music);
        await dbContext.SaveChangesAsync();
    }


    private (string Title, string Artist, string Length, string Date, string Genre) ExtractMetaData(FFProbeResult metadata)
    {
        string title = string.Empty;
        string artist = string.Empty;
        string length = string.Empty;
        string date = string.Empty;
        string genre = string.Empty;
        length = metadata.Format.Duration;
        string duration = FormatDuration(length);
        foreach (var str in metadata.Format.Tags)
        {
            if (str.Key == "title")
                title = str.Value;
            if (str.Key == "artist")
                artist = str.Value;
            if (str.Key == "date")
                date = str.Value;
        }
        return (title, artist, duration, date, genre);
    }

    private string FormatDuration(string? durationString)
    {
        if (string.IsNullOrWhiteSpace(durationString))
            return "0:00";

        if (!double.TryParse(durationString, out var seconds))
            return "0:00";

        var ts = TimeSpan.FromSeconds(seconds);
        return $"{(int)ts.TotalMinutes}:{ts.Seconds:D2}";
    }

}
