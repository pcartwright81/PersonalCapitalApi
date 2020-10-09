using System;
using System.Runtime.Serialization;

namespace PersonalCapital.Exceptions
{
    /// <summary>
    ///     Base exception for all PersonalCapital responses.
    /// </summary>
    public class PersonalCapitalException : Exception
    {
        public PersonalCapitalException()
        {
        }

        public PersonalCapitalException(string message) : base(message)
        {
        }

        public PersonalCapitalException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected PersonalCapitalException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}