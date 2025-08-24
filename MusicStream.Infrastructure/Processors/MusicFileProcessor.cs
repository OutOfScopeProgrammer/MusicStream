using Microsoft.Extensions.DependencyInjection;
using MusicStream.Application.Interfaces;
using MusicStream.Domain.Entities;
using MusicStream.Infrastructure.FileManagement;
using MusicStream.Infrastructure.Persistence.Postgres;

namespace MusicStream.Infrastructure.Processors;

internal class MusicFileProcessor(FFProcessor ffProcessor, IMusicStorage musicStorage,
IServiceScopeFactory scopeFactory)
{

    public async Task ProcessAsync(string filePath, string outputFolder, string rootPath)
    {
        var metaData = await ffProcessor.ConvertForDash(filePath, outputFolder);
        var fileManager = new FileManager();

        var response = await fileManager.GetFilesFromDirectory(outputFolder);
        if (response.IsSuccess)
        {
            await musicStorage.BatchUploadToMinio(response.Data, rootPath);
            var fileName = Path.GetFileNameWithoutExtension(filePath);

            var streamUrl = $"{fileName}/manifest.mpd";
            await SaveMusic(metaData!, streamUrl);

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
        string date = string.Empty;
        string genre = string.Empty;
        string duration = FormatDuration(metadata.Format.Duration);
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
