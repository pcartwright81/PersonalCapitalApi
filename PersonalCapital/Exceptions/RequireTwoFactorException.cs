namespace PersonalCapital.Exceptions;

public class RequireTwoFactorException : PersonalCapitalException
{
    public RequireTwoFactorException() : base("Two-factor authentication is required")
    {
    }
}