using System.Diagnostics.Metrics;

namespace MusicStream.Infrastructure.Observability;

public static class FFMeter
{
    public const string FFMPEGSPROCESS = "MusicStream.FFmpeg";

    private static readonly Meter _meter = new(FFMPEGSPROCESS);

    public static readonly Histogram<double> FFmpegDuration = _meter.CreateHistogram<double>("ffmpeg_duration_seconds");
    public static readonly Counter<long> FFmpegCounter = _meter.CreateCounter<long>("ffmpeg_total_job");
    public static readonly Histogram<double> FFprobeDuration = _meter.CreateHistogram<double>("ffprobe_duration_seconds");
    public static readonly Counter<long> FFprobeCounter = _meter.CreateCounter<long>("ffprobe_total_job");


}
