using System.Threading.Tasks;
using Courier.AspNetCoreSample.MessageTypes;
using Microsoft.Extensions.Logging;

namespace Courier.AspNetCoreSample.Receivers
{
    public class TextMessageReceiver : HostedCourierReceiverBase<TextMessage>
    {
        public TextMessageReceiver(ILogger<CourierReceiverBase<TextMessage>> logger, ICourier courier) 
            : base(logger, courier)
        {
        }

        protected override Task Execute(TextMessage message)
        {
            _logger.LogInformation("Got message: {0}", message.Contents);
            return Task.CompletedTask;
        }
    }
}