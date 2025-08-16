using System.Threading.Channels;
using MusicStream.Application.Interfaces;

namespace MusicStream.Infrastructure.BackgroundServices;

public class MusicChannel : IMusicChannel
{
    private Channel<string> _channel;
    public MusicChannel()
    {
        var option = new UnboundedChannelOptions()
        {
            SingleReader = true,
            SingleWriter = false
        };
        _channel = Channel.CreateUnbounded<string>(option);
    }


    public async Task SendAsync(string item)
        => await _channel.Writer.WriteAsync(item);
    public async Task<bool> WaitToReadAsync() => await _channel.Reader.WaitToReadAsync();
    public async Task ReadAsync()
        => await _channel.Reader.ReadAsync();




}
