using System;

namespace Courier
{
    public class CourierSubscriptionBuilder<TEvent>
        where TEvent : class, ICourierEvent
    {
        private Action<TEvent>? _action = null;
        private Func<TEvent, bool>? _filter = null;

        public CourierSubscriptionBuilder()
        {
        }

        internal CourierSubscription<TEvent> BuildWithStream(IObservable<ICourierEvent> eventStream)
        {
            if(_action is null)
                throw new CourierException($"An action must be specified for handling events of type {typeof(TEvent).Name}");

            if(_filter is null)
                _filter = (e) => true;

            return new CourierSubscription<TEvent>(eventStream, _action, _filter);
        }

        public CourierSubscriptionBuilder<TEvent> WithFilter(Func<TEvent, bool> filter)
        {
            _filter = filter;

            return this;
        }

        public CourierSubscriptionBuilder<TEvent> WithAction(Action<TEvent> action)
        {
            _action = action;

            return this;
        }

        internal CourierSubscriptionBuilder<TEvent> FromFactoryAction<TEventListener>(Func<TEventListener> factoryAction)
            where TEventListener : ICourierListener<TEvent>
        {
            _action = e => factoryAction.Invoke().Process(e);

            return this;
        }

        internal CourierSubscriptionBuilder<TEvent> FromListenerInstance<TEventListener>(TEventListener instance)
            where TEventListener : ICourierListener<TEvent>
        {
            _action = (e) => instance.Process(e);

            return this;
        }
    }
}