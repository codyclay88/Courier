using System;

namespace Courier
{
    public class CourierException : Exception
    {
        public CourierException(string message) : base(message)
        {
        }
    }
}