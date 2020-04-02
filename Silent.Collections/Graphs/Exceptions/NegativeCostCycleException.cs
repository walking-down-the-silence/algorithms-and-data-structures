using System;

namespace Silent.Collections
{
    public class NegativeCostCycleException : Exception
    {
        public NegativeCostCycleException(string message) : base(message)
        {
        }

        public NegativeCostCycleException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public NegativeCostCycleException()
        {
        }
    }
}