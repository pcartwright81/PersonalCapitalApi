namespace PersonalCapital.Exceptions
{
    public class UnknownUserException : PersonalCapitalException
    {
        public UnknownUserException() : base("Supplied user is inactive")
        {
        }
    }
}