using System;

namespace Courier
{
    internal abstract class CourierSubscriberBase<TEvent>
        where TEvent : class, ICourierEvent
    {
        internal abstract Action<TEvent> Action { get; }
    }

    internal class CourierSubscriber<TEvent> : CourierSubscriberBase<TEvent>
        where TEvent : class, ICourierEvent
    {
        public CourierSubscriber(Action<TEvent> action)
        {
            Action = action;
        }

        internal override Action<TEvent> Action { get; }
    }

    internal class CourierSubscriber<TEvent, TEventListener>
        : CourierSubscriberBase<TEvent>
        where TEvent : class, ICourierEvent
        where TEventListener : ICourierListener<TEvent>
    {
        public CourierSubscriber()
        {
            var listener = (TEventListener)Activator.CreateInstance(typeof(TEventListener));
            Action = (e) => listener.Process(e);
        }

        public CourierSubscriber(TEventListener instance)
        {
            Action = (e) => instance.Process(e);
        }

        public CourierSubscriber(CourierListenerFactory<TEvent, TEventListener> listenerFactory)
        {
            Action = (e) => listenerFactory.Factory.Invoke().Process(e);
        }

        internal override Action<TEvent> Action { get; }
    }
}