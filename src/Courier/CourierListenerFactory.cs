using System;

namespace Courier
{
    public class CourierListenerFactory<TEvent, TEventListener>
        where TEvent : class, ICourierEvent
        where TEventListener : ICourierListener<TEvent>
    {
        public CourierListenerFactory(Func<TEventListener> factory)
        {
            Factory = factory;
        }

        internal Func<TEventListener> Factory { get; }
    }
}