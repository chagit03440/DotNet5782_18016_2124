using System;
using System.Runtime.Serialization;

namespace IBL.BO
{
    [Serializable]
    public class BLDroneException : Exception
    {
        public BLDroneException()
        {
        }

        public BLDroneException(string message) : base(message)
        {
        }

        public BLDroneException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BLDroneException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}