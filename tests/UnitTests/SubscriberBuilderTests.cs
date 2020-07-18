using Xunit;

namespace Courier.UnitTests
{
    public class SubscriberBuilderTests
    {
        private ICourier _courier = new InMemoryCourier();

        [Fact]
        public void ThrowsExceptionIfNoActionIsSpecified()
        {
            var builder = new CourierSubscriptionBuilder<SomeEvent>();

            Assert.Throws<CourierException>(() => 
                _courier.Subscribe<SomeEvent>(builder)
            );
        }

        [Fact]
        public void CanSpecifyFilter()
        {
            var i = 0;
            var builder = new CourierSubscriptionBuilder<NumberedEvent>();

            builder.WithFilter((e) => e.Number > 10).WithAction((e) => i = i + 1);

            _courier.Subscribe(builder);

            _courier.Dispatch(new NumberedEvent(5));
            _courier.Dispatch(new NumberedEvent(15));

            Assert.Equal(1, i);
        }
    }
}