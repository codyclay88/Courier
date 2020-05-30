using System.Threading.Tasks;
using Courier.AspNetCoreSample.MessageTypes;
using Microsoft.Extensions.Logging;

namespace Courier.AspNetCoreSample.Receivers
{
    public class TextMessageReceiver : HostedCourierReceiverBase<TextMessage>
    {
        public TextMessageReceiver(
            ICourier courier,
            ILogger<TextMessageReceiver> logger) 
            : base(courier, logger)
        {
        }

        protected override Task Execute(TextMessage message)
        {
            _logger.LogInformation("Got message: {0}", message.Contents);
            return Task.CompletedTask;
        }
    }
}