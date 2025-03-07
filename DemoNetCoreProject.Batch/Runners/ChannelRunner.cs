using Microsoft.Extensions.Logging;
using System.Threading.Channels;

namespace DemoNetCoreProject.Batch.Runners
{
    public class ChannelRunner(ILogger<ChannelRunner> _logger)
    {
        private readonly Channel<int> channels = Channel.CreateUnbounded<int>();
        public Channel<int> Channels => channels;
        public int Count = 0;
        public async Task Producer() {
            await Task.Delay(1000);
            await channels.Writer.WriteAsync(++Count);
        }
        public async Task Consumer(int id)
        {
            await Task.Delay(new Random().Next(0, 1000));
            if (channels.Reader.Count > 0)
            {
                var value = await channels.Reader.ReadAsync();
                if (new Random().Next(0, 2) == 0)
                {
                    _logger.LogInformation("Consumer {id}: {Value}", id, value);
                }
                else
                {
                    await Task.Delay(1000);
                    await channels.Writer.WriteAsync(value);
                }
            }
        }
    }
}
