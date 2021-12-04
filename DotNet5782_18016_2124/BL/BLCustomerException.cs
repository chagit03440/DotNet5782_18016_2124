using System;
using System.Runtime.Serialization;

namespace IBL.BO
{
    [Serializable]
    public class BLCustomerException : Exception
    {
        public BLCustomerException()
        {
        }

        public BLCustomerException(string message) : base(message)
        {
        }

        public BLCustomerException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BLCustomerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}