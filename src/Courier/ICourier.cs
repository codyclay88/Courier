using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Courier
{
    public interface ICourier
    {
        IObservable<ICourierEvent> Events { get; }
        void Dispatch<TEvent>(TEvent @event) where TEvent : ICourierEvent;
        void Dispatch<TEvent>(params TEvent[] events) where TEvent : ICourierEvent;
        void Dispatch<TEvent>(IEnumerable<TEvent> events) where TEvent : ICourierEvent;
        void Subscribe<TEvent>(Action<TEvent> action) where TEvent : class, ICourierEvent;
        void Subscribe<TEvent, TEventListener>()
            where TEvent : class, ICourierEvent
            where TEventListener : ICourierListener<TEvent>;

        void Subscribe<TEvent, TEventListener>(Func<TEventListener> factoryAction)
            where TEvent : class, ICourierEvent
            where TEventListener : ICourierListener<TEvent>;

        void Subscribe<TEvent, TEventListener>(CourierListenerFactory<TEvent, TEventListener> factory)
            where TEvent : class, ICourierEvent
            where TEventListener : ICourierListener<TEvent>;
    }
}