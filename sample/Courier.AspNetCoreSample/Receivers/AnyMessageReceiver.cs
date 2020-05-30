using System.Threading.Tasks;
using Courier.AspNetCoreSample.MessageTypes;
using Microsoft.Extensions.Logging;

namespace Courier.AspNetCoreSample.Receivers
{
    public class AnyMessageReceiver : HostedCourierReceiverBase
    {
        public AnyMessageReceiver(ICourier courier, ILogger<CourierBase> logger) : base(courier, logger)
        {
        }

        public override Task Execute(IMessage message)
        {
            var output = message switch {
                TextMessage _ => "We got a TextMessage!",
                _ => "Unknown message type..."
            };
            _logger.LogInformation(output);
            return Task.CompletedTask;
        }
    }
}