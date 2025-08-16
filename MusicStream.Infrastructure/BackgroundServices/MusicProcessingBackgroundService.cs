using Microsoft.Extensions.Hosting;
using MusicStream.Application.Interfaces;

namespace MusicStream.Infrastructure.BackgroundServices;

public class MusicProcessingBackgroundService(IMusicChannel channel) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            Console.WriteLine("something");

            if (await channel.WaitToReadAsync())
            {
                // TODO: Process music and put it in the minio
            }

        }
    }
}
