using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MusicStream.Application.Interfaces;
using MusicStream.Domain.Entities;
using MusicStream.Infrastructure.FileManagement;
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
        var watcher = new FileWatcherBuilder().FolderToWatch(ROOTFOLDER).Filter("*.mp3").Build();
        watcher.Start();
        while (!stoppingToken.IsCancellationRequested)
        {

            if (await channel.WaitToReadAsync())
            {
                try
                {
                    var fileManager = new FileManager();
                    while (watcher.CreatedFiles.TryDequeue(out var filePath))
                    {
                        if (!await fileManager.IsFileReady(filePath))
                            continue;
                        var task = Task.Run(async () =>
                        {
                            var outputFolder = Path.Combine(ROOTFOLDER, Path.GetFileNameWithoutExtension(filePath));
                            Directory.CreateDirectory(outputFolder);

                            var metaData = await musicProcessor.ConvertForDash(filePath, outputFolder);

                            var files = await fileManager.GetFilesFromDirectory(outputFolder);
                            Console.WriteLine("Sending to minio....");

                            await musicStorage.BatchUploadToMinio(files, ROOTFOLDER);


                            //     var streamUrl = $"{msg.StoredName}/manifest.mpd";
                            //     await SaveMusic(metaData!, streamUrl);

                            //     await Task.Run(() =>
                            //    {
                            //        File.Delete(msg.TempFilePath);
                            //        Directory.Delete(outputFolder, true);
                            //    }, stoppingToken);
                        });
                    }
                    var msg = await channel.ReadAsync();

                    Console.WriteLine("Processing....");


                }
                catch (Exception)
                {
                    throw;
                }
            }

        }
    }


    private async Task SaveMusic(FFProbeResult metaData, string streamUrl)
    {
        using var scope = scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        (string title, string artist, string duration, string date, string genre) = ExtractMetaData(metaData);
        var music = Music.Create(title, artist, date, duration, genre, true, streamUrl);
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
            if (str.Key == "genre")
                genre = str.Value;
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
