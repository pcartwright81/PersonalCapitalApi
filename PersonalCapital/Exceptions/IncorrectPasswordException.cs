namespace PersonalCapital.Exceptions
{
    public class IncorrectPasswordException : PersonalCapitalException
    {
        public IncorrectPasswordException(string message) : base(message)
        {
        }
    }
}