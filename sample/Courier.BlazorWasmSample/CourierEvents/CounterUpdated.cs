using Courier;

namespace BlazorWasmSample
{
    public class CounterUpdated : ICourierEvent
    {
        public CounterUpdated(int counter)
        {
            Counter = counter;
        }

        public int Counter { get; }
    }
}