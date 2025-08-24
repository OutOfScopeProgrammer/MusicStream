using System.Diagnostics;
using System.Text;
using System.Text.Json;
using MusicStream.Infrastructure.Observability;

namespace MusicStream.Infrastructure.Processors
{

    internal record FFprobeFormat(string FileName, string Duration, Dictionary<string, string> Tags);

    internal record StreamInfo(string CodecName, string CodecType, string SampleRate, int Channels, string BitRate, string Duration);
    internal record FFProbeResult(List<StreamInfo> Streams, FFprobeFormat Format);




    internal class FFProcessor
    {
        private const string FFMPEGPATH = @"C:\Users\rezaj\AppData\Local\Microsoft\WinGet\Packages\Gyan.FFmpeg.Essentials_Microsoft.Winget.Source_8wekyb3d8bbwe\ffmpeg-7.1.1-essentials_build\bin\ffmpeg.exe";
        private const string FFPROBEPATH = @"C:\Users\rezaj\AppData\Local\Microsoft\WinGet\Packages\Gyan.FFmpeg.Essentials_Microsoft.Winget.Source_8wekyb3d8bbwe\ffmpeg-7.1.1-essentials_build\bin\ffprobe.exe";

        public async Task<FFProbeResult?> ConvertForDash(string inputFile, string outputFolder)
        {

            var sb = new StringBuilder();
            sb.Append($"-i \"{inputFile}\" ")
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
            .Append($"\"{outputFolder}\"/manifest.mpd");
            string FFMPEGARGS = sb.ToString();
            var metadata = await RunFFProble(inputFile);

            await RunFFmpeg(FFMPEGARGS);
            return metadata;
        }


        private async Task RunFFmpeg(string ffmpegArguments)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

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
                FFMeter.FFmpegCounter.Add(1, new KeyValuePair<string, object?>("status", "failed"));

                Console.WriteLine(stderr);
                Console.WriteLine(stdout);
                throw new Exception($"FFmpeg exited with code {process.ExitCode}");
            }
            else
            {
                FFMeter.FFmpegCounter.Add(1, new KeyValuePair<string, object?>("status", "success"));
            }


            stopWatch.Stop();
            FFMeter.FFmpegDuration.Record(stopWatch.Elapsed.TotalSeconds);

        }

        private async Task<FFProbeResult?> RunFFProble(string inputFile)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();



            var args = $"-v quiet -print_format json -show_format -show_streams \"{inputFile}\"";

            using var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = FFPROBEPATH,
                    Arguments = args,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();

            var stderr = process.StandardError.ReadToEndAsync();
            var stdout = process.StandardOutput.ReadToEndAsync();

            await process.WaitForExitAsync();
            string output = await stdout;
            string error = await stderr;


            if (process.ExitCode != 0)
            {
                FFMeter.FFprobeCounter.Add(1, new KeyValuePair<string, object?>("status", "failed"));

                Console.WriteLine(stderr);
                Console.WriteLine(stdout);

                throw new Exception($"FFProbe exited with code {process.ExitCode}");
            }
            else
            {
                FFMeter.FFprobeCounter.Add(1, new KeyValuePair<string, object?>("status", "success"));

            }

            var metaData = JsonSerializer.Deserialize<FFProbeResult>(output,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            stopWatch.Stop();
            FFMeter.FFprobeDuration.Record(stopWatch.Elapsed.TotalSeconds);

            return metaData;
        }
    }
}
