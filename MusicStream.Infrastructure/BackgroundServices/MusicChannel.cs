using System.Threading.Channels;
using MusicStream.Application;
using MusicStream.Application.Interfaces;

namespace MusicStream.Infrastructure.BackgroundServices;

public class MusicChannel : IMusicChannel
{
    private Channel<ChannelDto> _channel;
    public MusicChannel()
    {
        var option = new UnboundedChannelOptions()
        {
            SingleReader = true,
            SingleWriter = false
        };
        _channel = Channel.CreateUnbounded<ChannelDto>(option);
    }


    public async Task SendAsync(ChannelDto item)
        => await _channel.Writer.WriteAsync(item);
    public async Task<bool> WaitToReadAsync() => await _channel.Reader.WaitToReadAsync();
    public async Task<ChannelDto> ReadAsync()
        => await _channel.Reader.ReadAsync();




}
