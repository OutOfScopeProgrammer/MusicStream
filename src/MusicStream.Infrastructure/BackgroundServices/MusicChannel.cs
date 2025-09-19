using System.Threading.Channels;
using MusicStream.Application.Interfaces;

namespace MusicStream.Infrastructure.BackgroundServices;


internal class MusicChannel : IMusicChannel
{
    private Channel<MusicChannelMessage> _channel;
    public MusicChannel()
    {
        var option = new UnboundedChannelOptions()
        {
            SingleReader = true,
            SingleWriter = false
        };
        _channel = Channel.CreateUnbounded<MusicChannelMessage>(option);
    }


    public async Task SendAsync(MusicChannelMessage item)
        => await _channel.Writer.WriteAsync(item);
    public async Task<bool> WaitToReadAsync() => await _channel.Reader.WaitToReadAsync();
    public async Task<MusicChannelMessage> ReadAsync()
        => await _channel.Reader.ReadAsync();




}
