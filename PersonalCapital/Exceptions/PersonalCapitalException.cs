using System;

namespace PersonalCapital.Exceptions
{
    /// <summary>
    ///     Base exception for all PersonalCapital responses.
    /// </summary>
    public class PersonalCapitalException : Exception
    {
        public PersonalCapitalException(string message) : base(message)
        {
        }
    }
}