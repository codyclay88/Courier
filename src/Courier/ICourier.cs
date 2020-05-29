using System;

namespace Courier
{
    public interface ICourier
    {
        IObservable<IMessage> Messages { get; }

        void Send(IMessage message);
    }
}