using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Microsoft.Extensions.Logging;

namespace Courier
{
    public class Courier : ICourier
    {
        private ReplaySubject<IMessage> _messages = new ReplaySubject<IMessage>();
        private ILogger<ICourier> _logger;

        public Courier(ILogger<ICourier> logger)
        {
            _logger = logger;
        }

        public void Send(IMessage message)
        {
            _messages.OnNext(message);
        }

        public IObservable<IMessage> Messages => _messages.AsObservable();
    }
}