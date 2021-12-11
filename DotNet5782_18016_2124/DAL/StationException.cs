using System;
using System.Runtime.Serialization;

namespace DO
{
    [Serializable]
    public class StationException : Exception
    {
        public StationException() : base() { }

        public StationException(string message) : base(message) { }

        public StationException(string message, Exception innerException) : base(message, innerException) { }

        public override string ToString()
        {
            return Message;
        }
    }
}
