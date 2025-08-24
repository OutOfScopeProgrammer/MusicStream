using Microsoft.Extensions.Hosting;
using MusicStream.Infrastructure.FileManagement;
using MusicStream.Infrastructure.Processors;

namespace MusicStream.Infrastructure.BackgroundServices;

internal class MusicProcessingBackgroundService
(MusicFileProcessor processor) : BackgroundService
{
    private const string ROOTFOLDER = @"E:\ASP.NET\MusicStream\Music.APi\wwwroot";
    private readonly FileManager fileManager = new();

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var watcher = new FileWatcherBuilder()
            .FolderToWatch(ROOTFOLDER)
            .Filter("*.mp3")
            .Build();
        watcher.Start();


        var concurrencyLimit = new SemaphoreSlim(4);

        while (!stoppingToken.IsCancellationRequested)
        {
            await watcher._semaphore.WaitAsync(stoppingToken);


            while (watcher._createdFiles.TryDequeue(out var filePath))
            {


                if (!await fileManager.IsFileReady(filePath))
                    continue;

                try
                {
                    Console.WriteLine($"WaitAsync: {concurrencyLimit.CurrentCount}");
                    await concurrencyLimit.WaitAsync(stoppingToken);
                    var task = Process(filePath);
                    await task;


                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    throw;
                }
                finally
                {
                    concurrencyLimit.Release();
                    Console.WriteLine($"Release : {concurrencyLimit.CurrentCount}");
                }

            }

        }
    }
    private string CreateOutputDirectory(string rootPath, string filePath)
    {
        var fileName = Path.GetFileNameWithoutExtension(filePath);
        var outputFolder = Path.Combine(rootPath, fileName);
        Directory.CreateDirectory(outputFolder);
        return outputFolder;
    }

    private async Task Process(string filePath)
    {
        var outputFolder = CreateOutputDirectory(ROOTFOLDER, filePath);
        await processor.ProcessAsync(filePath, outputFolder, ROOTFOLDER);
        fileManager.DeleteSingleFile(filePath);
        fileManager.DeleteSingleDirectory(outputFolder);
    }
}