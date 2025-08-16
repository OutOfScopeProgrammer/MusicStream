using System.Diagnostics;

namespace MusicStream.Infrastructure;

internal class MusicProcessor
{
    private const string FFMPEGPATH = @"C:\Users\rezaj\AppData\Local\Microsoft\WinGet\Packages\Gyan.FFmpeg.Essentials_Microsoft.Winget.Source_8wekyb3d8bbwe\ffmpeg-7.1.1-essentials_build\bin\ffmpeg.exe";



    public async Task Convert(string tempFilePath, string fileName)
    {
        var task1 = ConvertTo128Kb(tempFilePath, fileName);
        var task2 = ConvertTo256Kb(tempFilePath, fileName);
        var task3 = ConvertTo320Kb(tempFilePath, fileName);
        await Task.WhenAll(task1, task2, task3);
    }
    private async Task ConvertTo128Kb(string tempFilePath, string fileName)
    {
        const string BITRATE_128 = "128k";

        var directory = Path.Combine(fileName, BITRATE_128);
        Directory.CreateDirectory(directory);
        await FFmpegProceess(tempFilePath, BITRATE_128, directory);

    }

    private async Task ConvertTo256Kb(string tempFilePath, string fileName)
    {
        const string BITRATE_256 = "256k";
        var directory = Path.Combine(fileName, BITRATE_256); ;
        Directory.CreateDirectory(directory);
        await FFmpegProceess(tempFilePath, BITRATE_256, directory);
    }
    private async Task ConvertTo320Kb(string tempFilePath, string fileName)
    {
        const string BITRATE_320 = "320k";

        var directory = Path.Combine(fileName, BITRATE_320); ;
        Directory.CreateDirectory(directory);
        await FFmpegProceess(tempFilePath, BITRATE_320, directory);
    }


    private async Task FFmpegProceess(string inputFile, string bitrate, string directory)
    {
        const string PLAYLIST = "playlist.m3u8";
        const string SEGMENTFORMAT = "audio_%03d.ts";
        string segmentPattern = Path.Combine(directory, SEGMENTFORMAT);
        string playlist = Path.Combine(directory, PLAYLIST);
        string args = $"-i \"{inputFile}\" -c:a aac -b:a {bitrate} -f hls -hls_time 6 -hls_playlist_type vod -hls_segment_filename \"{segmentPattern}\" \"{playlist}\"";
        var process = new Process()
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = FFMPEGPATH,
                Arguments = args,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true

            }
        };
        process.Start();
        await process.WaitForExitAsync();
        if (process.ExitCode != 0)
            throw new Exception("ffmpeg processing failed");
    }
}
