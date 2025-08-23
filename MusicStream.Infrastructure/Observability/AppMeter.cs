using System.Diagnostics.Metrics;

namespace MusicStream.Infrastructure.Observability;

public static class AppMeters
{
    public const string FFMPEGSPROCESS = "MusicStream.FFmpeg";
    public const string FFPROBEPROCESS = "MusicStream.FFprobe";
    public const string MUSICBACKGROUNDSERVICE = "MusicStream.MusicBackgroundService";

    public static readonly Meter FFmpegMeter = new Meter(FFMPEGSPROCESS);
    public static readonly Meter FFprobeMeter = new Meter(FFPROBEPROCESS);
    public static readonly Meter MusicBackgroundService = new Meter(MUSICBACKGROUNDSERVICE);

}
