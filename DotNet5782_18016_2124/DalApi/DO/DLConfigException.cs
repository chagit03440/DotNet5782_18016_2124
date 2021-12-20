using System;
using System.Runtime.Serialization;

namespace DalApi
{
    [Serializable]
    public class DLConfigException : Exception
    {
        public DLConfigException()
        {
        }

        public DLConfigException(string message) : base(message)
        {
        }

        public DLConfigException(string message, Exception innerException) : base(message, innerException)
        {
        }

       
        protected DLConfigException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}