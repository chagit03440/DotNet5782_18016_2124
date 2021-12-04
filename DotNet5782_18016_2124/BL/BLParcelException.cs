using System;
using System.Runtime.Serialization;

namespace IBL.BO
{
    [Serializable]
    public class BLParcelException : Exception
    {
        public BLParcelException()
        {
        }

        public BLParcelException(string message) : base(message)
        {
        }

        public BLParcelException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BLParcelException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}