namespace Pacco.Services.Identity.Core.Exceptions
{
    public class InvalidPasswordException : ExceptionBase
    {
        public override string Code => "invalid_password";

        public InvalidPasswordException() : base($"Invalid password.")
        {
        }
    }
}