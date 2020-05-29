using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Courier
{
    public abstract class CourierReceiverBase<T>
        : ICourierReceiver where T : class, IMessage
    {
        protected readonly ICourier _courier;
        protected readonly ILogger<CourierReceiverBase<T>> _logger;
        private IDisposable _subscription;

        protected Func<T, bool> Filter = (message) => true;

        public CourierReceiverBase(
            ILogger<CourierReceiverBase<T>> logger,
            ICourier courier)
        {
            _logger = logger;
            _courier = courier;
        }

        public Task StartListening(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Started {0} processor...", typeof(T).Name);
            _subscription = _courier.Messages
                .OfType<T>()
                .Where(Filter)
                .Do((source) => BeforeExecute(source))
                .Do((source) => Execute(source))
                .Do((source) => AfterExecute(source))
                .Subscribe();

            return Task.CompletedTask;
        }

        public Task StopListening(CancellationToken cancellationToken)
        {
            _subscription.Dispose();
            return Task.CompletedTask;
        }

        protected abstract Task Execute(T message);
        protected virtual Task BeforeExecute(T message)
        {
            return Task.CompletedTask;
        }

        protected virtual Task AfterExecute(T message)
        {
            return Task.CompletedTask;
        }
    }
}