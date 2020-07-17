using Courier;

namespace Courier.AspNetCoreSample.MessageTypes
{
    public class SomethingHappenedEvent : ICourierEvent
    {
        public SomethingHappenedEvent(string contents)
        {
            Contents = contents;
        }

        public string Contents { get; }
    }
}