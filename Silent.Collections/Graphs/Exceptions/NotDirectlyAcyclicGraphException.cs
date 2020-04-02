using System;

namespace Silent.Collections
{
    public class NotDirectlyAcyclicGraphException : Exception
    {
        public NotDirectlyAcyclicGraphException(string message) : base(message)
        {
        }

        public NotDirectlyAcyclicGraphException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public NotDirectlyAcyclicGraphException()
        {
        }
    }
}