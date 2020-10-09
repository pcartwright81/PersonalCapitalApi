using System;
using System.Runtime.Serialization;

namespace PersonalCapital.Exceptions
{
    public class RequireTwoFactorException : PersonalCapitalException
    {
        public RequireTwoFactorException() : base("Two-factor authentication is required")
        {
        }

        public RequireTwoFactorException(string message) : base(message)
        {
        }

        public RequireTwoFactorException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RequireTwoFactorException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}