using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Courier.UnitTests
{
    public class CourierTests
    {
        private Courier _courier = new Courier();

        [Fact]
        public void CanSubscribeDirectlyToTheEventStream()
        {
            var i = 0;
            var sub = _courier.Events.Subscribe((e) => {
                i = i + 1;
            });

            _courier.Dispatch(new SomeEvent());
            Assert.Equal(1, i);
            sub.Dispose();
        }

        [Fact]
        public void CanSubscribeWithActions()
        {
            var i = 0;
            _courier.Subscribe<SomeEvent>((e) => i += 1);
            _courier.Dispatch(new SomeEvent());
            Assert.Equal(i, 1);
        }

        [Fact]
        public void CanSubscribeViaFactoryMethods()
        {
            _courier.Subscribe<SomeEvent, SomeEventListener>(() => new SomeEventListener());

            _courier.Dispatch(new SomeEvent());

            Assert.Equal(1, SomeEventListener.Y);
        }
        
        [Fact]
        public void CanSubscribeViaAnInstanceOfAListener()
        {
            var someEventListener = new SomeEventListener();
            _courier.Subscribe<SomeEvent, SomeEventListener>(someEventListener);
            _courier.Dispatch(new SomeEvent());
            Assert.Equal(1, SomeEventListener.Y);
        }
    }

    public class NumberedEvent : ICourierEvent
    {
        public NumberedEvent(int number)
        {
            Number = number;
        }

        public int Number { get; }
    }

    public class SomeOtherEventListener : ICourierListener<SomeEvent>
    {
        public static int X { get; private set; } = 0;

        public Task Process(SomeEvent @event)
        {
            X = 1;
            return Task.CompletedTask;
        }
    }

    public class SomeEventListener : ICourierListener<SomeEvent>
    {
        public static int Y { get; private set; } = 0;

        public Task Process(SomeEvent @event)
        {
            Y = 1;
            return Task.CompletedTask;
        }
    }

    public class SomeEvent : ICourierEvent
    {

    }
}
