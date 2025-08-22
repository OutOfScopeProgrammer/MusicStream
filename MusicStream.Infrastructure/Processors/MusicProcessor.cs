using System.Diagnostics;
using System.Text;

namespace MusicStream.Infrastructure.Processors
{
    internal class MusicProcessor
    {
        private const string FFMPEGPATH = @"C:\Users\rezaj\AppData\Local\Microsoft\WinGet\Packages\Gyan.FFmpeg.Essentials_Microsoft.Winget.Source_8wekyb3d8bbwe\ffmpeg-7.1.1-essentials_build\bin\ffmpeg.exe";


        public async Task ConvertForDash(string inputFile, string outputFolder)
        {
            var sb = new StringBuilder();
            sb.Append($"-i {inputFile} ")
            .Append($"-filter_complex ") // need space at the end
            .Append($"\"")
            .Append($"[0:a]aresample=44100[a1];")
            .Append($"[0:a]aresample=44100[a2];")
            .Append($"[0:a]aresample=44100[a3];")
            .Append($"\" ") // need space at the end 
            .Append($"-map \"[a1]\" -c:a aac -b:a 128k ")
            .Append($"-map \"[a2]\" -c:a aac -b:a 192k ")
            .Append($"-map \"[a3]\" -c:a aac -b:a 320k ")
            .Append($"-f dash ")
            .Append($"-seg_duration 5 ")
            .Append($"{outputFolder}/manifest.mpd");
            var args = sb.ToString();

            await RunFFmpeg(args);

        }


        private async Task RunFFmpeg(string ffmpegArguments)
        {
            using var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = FFMPEGPATH,
                    Arguments = ffmpegArguments,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();

            string stderr = await process.StandardError.ReadToEndAsync();
            string stdout = await process.StandardOutput.ReadToEndAsync();

            await process.WaitForExitAsync();

            if (process.ExitCode != 0)
            {
                Console.WriteLine("FFmpeg Error:");
                Console.WriteLine(stderr);
                Console.WriteLine(stdout);
                throw new Exception($"FFmpeg exited with code {process.ExitCode}");
            }
        }
    }
}
