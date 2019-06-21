namespace Pacco.Services.Identity.Core.Exceptions
{
    public class InvalidEmailException : ExceptionBase
    {
        public override string Code => "invalid_email";
        
        public InvalidEmailException(string email) : base($"Invalid email: {email}.")
        {
        }
    }
}