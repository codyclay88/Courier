using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Microsoft.Extensions.Logging;

namespace Courier
{
    public class Courier : ICourier, IDisposable
    {
        private ReplaySubject<ICourierEvent> _events = new ReplaySubject<ICourierEvent>();
        private List<IDisposable> _subscriptions = new List<IDisposable>();
        private readonly ILogger<Courier> _logger;

        public Courier(ILogger<Courier>? logger = null)
        {
            _logger = logger ?? new NoLogger();
        }

        public void Dispatch<TEvent>(TEvent @event)
            where TEvent : ICourierEvent
        {
            _logger.LogInformation($"Dispatching {typeof(TEvent).Name} event: {@event}");
            _events.OnNext(@event);
        }

        public void Dispatch<TEvent>(params TEvent[] events)
            where TEvent : ICourierEvent
        {
            foreach(var @event in events)
                this.Dispatch(@event);
        }

        public void Dispatch<TEvent>(IEnumerable<TEvent> events)
            where TEvent : ICourierEvent
        {
            this.Dispatch(events.ToArray());
        }

        internal void Subscribe<TEvent>(CourierSubscription<TEvent> subscription)
            where TEvent : class, ICourierEvent
        {
            _subscriptions.Add(subscription);
        }

        public void Subscribe<TEvent>(Action<TEvent> action)
            where TEvent : class, ICourierEvent
        {
            var subscription = new CourierSubscriptionBuilder<TEvent>().WithAction(action).BuildWithStream(Events);
            this.Subscribe(subscription);
        }

        public void Subscribe<TEvent, TEventListener>(TEventListener instance)
            where TEvent : class, ICourierEvent
            where TEventListener : ICourierListener<TEvent>
        {
            var subscription = new CourierSubscriptionBuilder<TEvent>().FromListenerInstance(instance).BuildWithStream(Events);
            this.Subscribe(subscription);
        }

        public void Dispose()
        {
            _subscriptions.ForEach((s) => s.Dispose());
            _events.Dispose();
        }

        public void Subscribe<TEvent, TEventListener>(Func<TEventListener> factoryAction)
            where TEvent : class, ICourierEvent
            where TEventListener : ICourierListener<TEvent>
        {
            var subscription = new CourierSubscriptionBuilder<TEvent>().FromFactoryAction(factoryAction).BuildWithStream(Events);
            this.Subscribe(subscription);
        }

        public void Subscribe<TEvent>(CourierSubscriptionBuilder<TEvent> builder)
            where TEvent : class, ICourierEvent
        {
            var subscriber = builder.BuildWithStream(Events);
            this.Subscribe(subscriber);
        }

        public IObservable<ICourierEvent> Events => _events.AsObservable();
    }
}