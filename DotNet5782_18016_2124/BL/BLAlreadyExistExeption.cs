using System;
using System.Runtime.Serialization;

namespace BO
{
    [Serializable]
    public class BLAlreadyExistExeption : Exception
    {
        public BLAlreadyExistExeption()
        {
        }

        public BLAlreadyExistExeption(string message) : base(message)
        {
        }

        public BLAlreadyExistExeption(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BLAlreadyExistExeption(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}