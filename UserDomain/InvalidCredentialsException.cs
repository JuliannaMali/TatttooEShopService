namespace UserDomain;

public class InvalidCredentialsException : Exception
{
    public InvalidCredentialsException() : base("Incorect Login or Password") { }
}
