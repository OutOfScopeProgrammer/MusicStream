using System.Diagnostics;

namespace MusicStream.Infrastructure.Processors;

internal class MusicProcessor
{
    private const string FFMPEGPATH = @"C:\Users\rezaj\AppData\Local\Microsoft\WinGet\Packages\Gyan.FFmpeg.Essentials_Microsoft.Winget.Source_8wekyb3d8bbwe\ffmpeg-7.1.1-essentials_build\bin\ffmpeg.exe";

    public async Task ConvertToHls(string accFile, string tempFilePath, string rootFolder, string fileName)
    {
        var task128 = ConvertTo128Kb(accFile, rootFolder, fileName);
        var task256 = ConvertTo256Kb(accFile, rootFolder, fileName);
        var task320 = ConvertTo320Kb(accFile, rootFolder, fileName);
        await Task.WhenAll(task128, task256, task320);
    }

    public async Task<string> ConvertToAcc(string tempFilePath, string rootFolder)
    {
        string fileName = Guid.NewGuid().ToString();
        var rootDirectory = Path.Combine(rootFolder, "Temp");
        Directory.CreateDirectory(rootDirectory);

        var outputFile = Path.Combine(rootDirectory, "temp.m4a");

        string ffmpegArguments = $"-i \"{tempFilePath}\" -vn -c:a aac -b:a 320k \"{outputFile}\"";
        var process = ProcessBuilder(ffmpegArguments);
        process.Start();
        string stderr = await process.StandardError.ReadToEndAsync();
        string stdout = await process.StandardOutput.ReadToEndAsync();

        await process.WaitForExitAsync();
        if (process.ExitCode != 0)
        {
            Console.WriteLine(stderr);
            Console.WriteLine(stdout);


            throw new Exception(process.ExitCode.ToString());
        }

        return outputFile;
    }
    private async Task ConvertTo128Kb(string tempFilePath, string rootFolder, string fileName)
    {
        const string BITRATE_128 = "128k";

        var rootDirectory = Path.Combine(rootFolder, fileName);
        var directory = Path.Combine(rootDirectory, BITRATE_128);
        Directory.CreateDirectory(directory);
        await FFmpegProceess(tempFilePath, BITRATE_128, directory);

    }

    private async Task ConvertTo256Kb(string tempFilePath, string rootFolder, string fileName)
    {
        const string BITRATE_256 = "256k";
        var rootDirectory = Path.Combine(rootFolder, fileName);
        var directory = Path.Combine(rootDirectory, BITRATE_256);
        Directory.CreateDirectory(directory);
        await FFmpegProceess(tempFilePath, BITRATE_256, directory);
    }
    private async Task ConvertTo320Kb(string tempFilePath, string rootFolder, string fileName)
    {
        const string BITRATE_320 = "320k";

        var rootDirectory = Path.Combine(rootFolder, fileName);
        var directory = Path.Combine(rootDirectory, BITRATE_320);
        Directory.CreateDirectory(directory);
        await FFmpegProceess(tempFilePath, BITRATE_320, directory);
    }


    private async Task FFmpegProceess(string inputFile, string bitrate, string directory)
    {
        const string PLAYLIST = "playlist.m3u8";
        const string SEGMENTFORMAT = "audio_%03d.ts";
        string segmentPattern = Path.Combine(directory, SEGMENTFORMAT);
        string playlist = Path.Combine(directory, PLAYLIST);

        string ffmpegArguments = $"-i \"{inputFile}\" -c:a aac -b:a {bitrate} -f hls -hls_time 6 -hls_playlist_type vod -hls_segment_filename \"{segmentPattern}\" \"{playlist}\"";

        var process = ProcessBuilder(ffmpegArguments);
        process.Start();
        string stderr = await process.StandardError.ReadToEndAsync();
        string stdout = await process.StandardOutput.ReadToEndAsync();

        await process.WaitForExitAsync();
        if (process.ExitCode != 0)
        {

            Console.WriteLine(stderr);
            Console.WriteLine(stdout);
            throw new Exception("ffmpeg processing failed");
        }
    }


    private Process ProcessBuilder(string ffmpegArguments)
    => new()
    {
        StartInfo = new ProcessStartInfo()
        {
            FileName = FFMPEGPATH,
            Arguments = ffmpegArguments,
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true

        }
    };


}
