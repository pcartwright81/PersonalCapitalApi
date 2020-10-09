using System;
using System.Runtime.Serialization;

namespace PersonalCapital.Exceptions
{
    public class IncorrectPasswordException : PersonalCapitalException
    {
        public IncorrectPasswordException() : base("Incorrect password")
        {
        }

        public IncorrectPasswordException(string message) : base(message)
        {
        }

        public IncorrectPasswordException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected IncorrectPasswordException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}