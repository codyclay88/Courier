using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Courier
{
    public abstract class CourierBase : ICourierReceiver
    {
        protected readonly ICourier _courier;
        protected readonly ILogger<CourierBase> _logger;

        public CourierBase(ICourier courier, ILogger<CourierBase> logger)
        {
            _courier = courier;
            _logger = logger;
        }

        public abstract Task StartListening(CancellationToken cancellationToken);
        public abstract Task StopListening(CancellationToken cancellationToken);
    }

    public abstract class CourierReceiverBase : CourierBase
    {
        private IDisposable _subscription;

        protected CourierReceiverBase(ICourier courier, ILogger<CourierBase> logger) : base(courier, logger)
        {
        }

        public override Task StartListening(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Started processor...");
            _subscription = _courier
                .Messages
                .Do((message) => Execute(message))
                .Subscribe();
            return Task.CompletedTask;
        }

        public abstract Task Execute(IMessage message);

        public override Task StopListening(CancellationToken cancellationToken)
        {
            _subscription.Dispose();
            return Task.CompletedTask;
        }
    }

    public abstract class CourierReceiverBase<T>
        : CourierBase where T : class, IMessage
    {
        private IDisposable _subscription;

        protected Func<T, bool> Filter = (message) => true;

        public CourierReceiverBase(
            ICourier courier,
            ILogger<CourierReceiverBase<T>> logger) : base(courier, logger)
        {

        }

        public override Task StartListening(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Started {0} processor...", typeof(T).Name);
            _subscription = _courier.Messages
                .OfType<T>()
                .Where(Filter)
                .Do((message) => BeforeExecute(message))
                .Do((message) => Execute(message))
                .Do((message) => AfterExecute(message))
                .Subscribe();

            return Task.CompletedTask;
        }

        public override Task StopListening(CancellationToken cancellationToken)
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