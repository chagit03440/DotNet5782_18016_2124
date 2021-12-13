//we did the bonus function
using System;
using System.Runtime.Serialization;

namespace DO
{
    [Serializable]
    public class InVaildIdException : Exception
    {
        public InVaildIdException()
        {
        }

        public InVaildIdException(string message) : base(message)
        {
        }

        public InVaildIdException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InVaildIdException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}