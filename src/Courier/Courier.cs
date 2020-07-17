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

        internal void Subscribe<TEvent>(CourierSubscriberBase<TEvent> subscriber)
            where TEvent : class, ICourierEvent
        {
            var subscription = CourierSubscription<TEvent>.FromSubscriber(subscriber, Events);
            _subscriptions.Add(subscription);
            _logger.LogInformation($"Added Courier subscription for event type: {typeof(TEvent).Name}");
        }

        public void Subscribe<TEvent>(Action<TEvent> action)
            where TEvent : class, ICourierEvent
        {
            var subscriber = new CourierSubscriber<TEvent>(action);
            this.Subscribe(subscriber);
        }

        public void Subscribe<TEvent, TEventListener>()
            where TEvent : class, ICourierEvent
            where TEventListener : ICourierListener<TEvent>
        {
            var subscriber = new CourierSubscriber<TEvent, TEventListener>();
            this.Subscribe(subscriber);
        }

        public void Subscribe<TEvent, TEventListener>(CourierListenerFactory<TEvent, TEventListener> factory)
            where TEvent : class, ICourierEvent
            where TEventListener : ICourierListener<TEvent>
        {
            var subscriber = new CourierSubscriber<TEvent, TEventListener>(factory);
            this.Subscribe(subscriber);
        }

        public void Subscribe<TEvent, TEventListener>(TEventListener instance)
            where TEvent : class, ICourierEvent
            where TEventListener : ICourierListener<TEvent>
        {
            var subscriber = new CourierSubscriber<TEvent, TEventListener>(instance);
            this.Subscribe(subscriber);
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
            var factory = new CourierListenerFactory<TEvent, TEventListener>(factoryAction);
            var subscriber = new CourierSubscriber<TEvent, TEventListener>(factory);
            this.Subscribe(subscriber);
        }

        public IObservable<ICourierEvent> Events => _events.AsObservable();
    }
}