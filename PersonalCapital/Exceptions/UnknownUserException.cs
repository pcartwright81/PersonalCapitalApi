using System;
using System.Runtime.Serialization;

namespace PersonalCapital.Exceptions
{
    public class UnknownUserException : PersonalCapitalException
    {
        public UnknownUserException() : base("Supplied user is inactive")
        {
        }

        public UnknownUserException(string message) : base(message)
        {
        }

        public UnknownUserException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnknownUserException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}