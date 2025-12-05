namespace PersonalCapital.Exceptions;

public class IncorrectPasswordException(string message) : PersonalCapitalException(message)
{
}