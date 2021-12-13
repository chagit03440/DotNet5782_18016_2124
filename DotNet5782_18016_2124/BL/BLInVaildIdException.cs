using System;
using System.Runtime.Serialization;

namespace IBL.BO
{
    [Serializable]
    public class BLInVaildIdException : Exception
    {
        public BLInVaildIdException()
        {
        }

        public BLInVaildIdException(string message) : base(message)
        {
        }

        public BLInVaildIdException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BLInVaildIdException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}