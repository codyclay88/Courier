using System.Threading.Tasks;
using Courier.AspNetCoreSample.MessageTypes;
using Microsoft.Extensions.Logging;

namespace Courier.AspNetCoreSample.Listeners
{
    public class SomethingHappenedEventListener : ICourierListener<SomethingHappenedEvent>
    {
        private readonly ILogger<SomethingHappenedEventListener> _logger;

        public SomethingHappenedEventListener(ILogger<SomethingHappenedEventListener> logger)
        {
            _logger = logger;
        }

        public Task Process(SomethingHappenedEvent @event)
        {
            _logger.LogInformation($"Something happened! {@event.Contents}");
            return Task.CompletedTask;
        }
    }
}