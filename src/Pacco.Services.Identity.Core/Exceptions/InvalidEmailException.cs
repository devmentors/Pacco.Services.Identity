namespace Pacco.Services.Identity.Core.Exceptions
{
    public class InvalidEmailException : DomainException
    {
        public override string Code => "invalid_email";
        
        public InvalidEmailException(string email) : base($"Invalid email: {email}.")
        {
        }
    }
}