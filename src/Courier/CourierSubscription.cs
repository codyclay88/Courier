using System;
using System.Linq;
using System.Reactive.Linq;

namespace Courier
{
    internal class CourierSubscription<TEvent> : IDisposable
        where TEvent : class, ICourierEvent
    {
        private IDisposable? _subscription;

        private CourierSubscription(IObservable<ICourierEvent> eventStream, Action<TEvent> action)
        {
            _subscription = eventStream
                .OfType<TEvent>()
                .Do(action)
                .Subscribe();
        }

        internal static CourierSubscription<TEvent> FromSubscriber(
            CourierSubscriberBase<TEvent> subscriber, IObservable<ICourierEvent> eventStream) 
        {
            return new CourierSubscription<TEvent>(eventStream, subscriber.Action);
        }

        public void Dispose()
        {
            _subscription?.Dispose();
        }
    }
}