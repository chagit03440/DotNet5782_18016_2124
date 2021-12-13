using System;
using System.Runtime.Serialization;

namespace DO
{
    [Serializable]
    public class AlreadyExistExeption : Exception
    {
        public AlreadyExistExeption()
        {
        }

        public AlreadyExistExeption(string message) : base(message)
        {
        }

        public AlreadyExistExeption(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AlreadyExistExeption(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}