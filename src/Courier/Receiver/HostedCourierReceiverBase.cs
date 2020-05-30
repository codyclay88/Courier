using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Courier
{
    public abstract class HostedCourierReceiverBase
        : CourierReceiverBase, IHostedService
    {
        protected HostedCourierReceiverBase(ICourier courier, ILogger<CourierBase> logger) : base(courier, logger)
        {
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return this.StartListening(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return this.StartListening(cancellationToken);
        }
    }

    public abstract class HostedCourierReceiverBase<T> :
        CourierReceiverBase<T>, 
        IHostedService 
        where T : class, IMessage
    {
        public HostedCourierReceiverBase(
            ICourier courier,
            ILogger<HostedCourierReceiverBase<T>> logger) 
            : base(courier, logger)
        {
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return this.StartListening(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return this.StopListening(cancellationToken);
        }
    }
}