using Microsoft.Extensions.Hosting;
using MusicStream.Application.Interfaces;
using MusicStream.Infrastructure.Processors;

namespace MusicStream.Infrastructure.BackgroundServices;

internal class MusicProcessingBackgroundService(IMusicChannel channel, MusicProcessor musicProcessor) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            Console.WriteLine("Processing....");

            if (await channel.WaitToReadAsync())
            {
                var dto = await channel.ReadAsync();
                var accFile = await musicProcessor.ConvertToAcc(dto.TempFilePath, dto.RootFolder);
                await musicProcessor.ConvertToHls(accFile, dto.TempFilePath, dto.RootFolder, dto.FileName);
                await CleanUpDisk(dto.TempFilePath);

                // TODO: Process music and put it in the minio
            }

        }
    }



    private async Task CleanUpDisk(string filePath)
    {
        var dir = Path.GetDirectoryName(filePath);
        if (Directory.Exists(dir))
            await Task.Run(() => Directory.Delete(dir, true));
    }
}
