using System.Runtime.CompilerServices;
using Microsoft.Extensions.Hosting;
using MusicStream.Infrastructure.FileManagement;
using MusicStream.Infrastructure.Processors;

namespace MusicStream.Infrastructure.BackgroundServices;

internal class MusicProcessingBackgroundService
(MusicFileProcessor processor) : BackgroundService
{
    private const string ROOTFOLDER = @"E:\ASP.NET\MusicStream\Music.APi\wwwroot";
    private readonly FileManager fileManager = new();
    private readonly SemaphoreSlim concurencyLimit = new(4);
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var watcher = new FileWatcherBuilder()
            .FolderToWatch(ROOTFOLDER)
            .Filter("*.mp3")
            .Build();
        watcher.Start();

        var processingTask = new List<Task>();


        while (!stoppingToken.IsCancellationRequested)
        {
            await watcher._semaphore.WaitAsync(stoppingToken);
            while (watcher._createdFiles.TryDequeue(out var filePath))
            {

                await concurencyLimit.WaitAsync(stoppingToken);

                var task = Task.Run(async () =>
                {
                    try
                    {
                        await ProcessFileAsync(filePath, stoppingToken);
                    }
                    finally
                    {
                        concurencyLimit.Release();
                    }
                });
                processingTask.RemoveAll(t => t.IsCompleted);
                processingTask.Add(task);
            }
        }
        await Task.WhenAll(processingTask);
    }
    private string CreateOutputDirectory(string rootPath, string filePath)
    {
        var fileName = Path.GetFileNameWithoutExtension(filePath);
        var outputFolder = Path.Combine(rootPath, fileName);
        Directory.CreateDirectory(outputFolder);
        return outputFolder;
    }

    private async Task ProcessFileAsync(string filePath, CancellationToken token)
    {
        try
        {
            if (await fileManager.IsFileReady(filePath))
            {
                var outputFolder = CreateOutputDirectory(ROOTFOLDER, filePath);
                await processor.ProcessAsync(filePath, outputFolder, ROOTFOLDER);
                fileManager.DeleteSingleFile(filePath);
                fileManager.DeleteSingleDirectory(outputFolder);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing {filePath}: {ex.Message}");
        }
    }
}