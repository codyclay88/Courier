using System;
using System.Linq;
using System.Reactive.Linq;

namespace Courier
{
    internal class CourierSubscription<TEvent> : IDisposable
        where TEvent : class, ICourierEvent
    {
        private IDisposable? _subscription;

        internal CourierSubscription(IObservable<ICourierEvent> eventStream, Action<TEvent> action)
            : this(eventStream, action, (e) => true)
        {

        }

        internal CourierSubscription(
            IObservable<ICourierEvent> eventStream, 
            Action<TEvent> action, 
            Func<TEvent, bool> filter)
        {
            _subscription = eventStream
                .OfType<TEvent>()
                .Where(filter)
                .Do(action)
                .Subscribe();
        }

        // internal static CourierSubscription<TEvent> FromSubscriber(
        //     CourierSubscriberBase<TEvent> subscriber, IObservable<ICourierEvent> eventStream) 
        // {
        //     return new CourierSubscription<TEvent>(eventStream, subscriber.Action);
        // }

        public void Dispose()
        {
            _subscription?.Dispose();
        }
    }
}