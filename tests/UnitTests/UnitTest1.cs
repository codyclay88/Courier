using System;
using System.Threading.Tasks;
using Xunit;

namespace Courier.UnitTests
{
    public class CourierTests
    {
        private Courier _courier = new Courier();

        [Fact]
        public void CanDirectlySubscribeWithActions()
        {
            var i = 0;
            //_courier.Subscribe<ICourierEvent>(e => i += 1);
            _courier.Subscribe<SomeEvent>((e) => i += 1);
            _courier.Dispatch(new SomeEvent());
            Assert.Equal(i, 1);
        }

        [Fact]
        public void CanDirectlySubscribeUsingOnlyTypeArguments()
        {            
            _courier.Subscribe<SomeEvent, SomeEventListener>();

            _courier.Dispatch(new SomeEvent());

            Assert.Equal(1, SomeEventListener.Y);
        }

        [Fact]
        public void CanDirectlySubscribeViaFactories()
        {
            var courierListenerFactory = new CourierListenerFactory<SomeEvent, SomeOtherEventListener>(() => new SomeOtherEventListener());
            _courier.Subscribe(courierListenerFactory);

            _courier.Dispatch(new SomeEvent());

            Assert.Equal(1, SomeOtherEventListener.X);
        }
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
        public string EventType => "Blah";

        public DateTime AuditDate => DateTime.Today;
    }
}
