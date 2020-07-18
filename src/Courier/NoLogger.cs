using System;
using Microsoft.Extensions.Logging;

namespace Courier
{
    public class NoLogger : ILogger<InMemoryCourier>
    {
        public IDisposable? BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return false;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            Console.WriteLine("I don't do anything.");
        }
    }
}