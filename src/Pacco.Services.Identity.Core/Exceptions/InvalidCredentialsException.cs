namespace Pacco.Services.Identity.Core.Exceptions
{
    public class InvalidCredentialsException : ExceptionBase
    {
        public override string Code => "invalid_credentials";

        public InvalidCredentialsException() : base("Invalid credentials.")
        {
        }
    }
}