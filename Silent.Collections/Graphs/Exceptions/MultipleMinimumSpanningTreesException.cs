using System;

namespace Silent.Collections
{
    public class MultipleMinimumSpanningTreesException : Exception
    {
        public MultipleMinimumSpanningTreesException(string message) : base(message)
        {
        }

        public MultipleMinimumSpanningTreesException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public MultipleMinimumSpanningTreesException()
        {
        }
    }
}