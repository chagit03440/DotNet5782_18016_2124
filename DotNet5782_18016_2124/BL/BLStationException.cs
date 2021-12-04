using System;
using System.Runtime.Serialization;

namespace IBL.BO
{
    [Serializable]
    public class BLStationException : Exception
    {
        public BLStationException()
        {
        }

        public BLStationException(string message) : base(message)
        {
        }

        public BLStationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BLStationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}