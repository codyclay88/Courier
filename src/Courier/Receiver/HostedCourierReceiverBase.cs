using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Courier
{
    public abstract class HostedCourierReceiverBase<T> :
        CourierReceiverBase<T>, 
        IHostedService 
        where T : class, IMessage
    {
        public HostedCourierReceiverBase(ILogger<CourierReceiverBase<T>> logger, ICourier courier) 
            : base(logger, courier)
        {
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Listening for {0} messages...", typeof(T).Name);
            return this.StartListening(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return this.StopListening(cancellationToken);
        }
    }
}