using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Courier
{
    public interface ICourier
    {
        ///<summary>
        /// Observable stream of all the events that flow through the given ICourier instance.
        ///</summary>
        IObservable<ICourierEvent> Events { get; }

        ///<summary>
        /// Dispatches an instance of an <see cref="ICourierEvent" /> into the event stream.
        ///</summary>
        void Dispatch<TEvent>(TEvent @event) where TEvent : ICourierEvent;

        ///<summary>
        /// Dispatches one or more instances of a given <see cref="ICourierEvent" /> into the event stream.
        ///</summary>
        void Dispatch<TEvent>(params TEvent[] events) where TEvent : ICourierEvent;

        ///<summary>
        /// Dispatches an enumerable of <see cref="ICourierEvent" /> values into the event stream, where each individual 
        /// instance will be dispatched independently.
        ///</summary>
        void Dispatch<TEvent>(IEnumerable<TEvent> events) where TEvent : ICourierEvent;

        ///<summary>
        /// Registers an Action to be executed whenever a <c>TEvent</c> is dispatched into the event stream.
        ///</summary>
        void Subscribe<TEvent>(Action<TEvent> action) where TEvent : class, ICourierEvent;

        ///<summary>
        /// Registers a subscriber of type <c>TEventListener</c> where a new instance is 
        /// generated via the <paramref name="factoryAction" />
        /// in response to a new <c>TEvent</c> instance coming into the event stream.
        ///</summary>
        void Subscribe<TEvent, TEventListener>(Func<TEventListener> factoryAction)
            where TEvent : class, ICourierEvent
            where TEventListener : ICourierListener<TEvent>;

        ///<summary>
        /// Registers a <c>TEventListener</c> instance that will events of type <c>TEvent</c>
        ///</summary>
        void Subscribe<TEvent, TEventListener>(TEventListener instance)
            where TEvent : class, ICourierEvent
            where TEventListener : ICourierListener<TEvent>;

        ///<summary>
        /// Registers a <c>TEventListener</c> created from the provided <see cref="CourierSubscriptionBuilder{TEvent}" />
        ///</summary>
        void Subscribe<TEvent>(CourierSubscriptionBuilder<TEvent> builder)
            where TEvent : class, ICourierEvent;
    }
}