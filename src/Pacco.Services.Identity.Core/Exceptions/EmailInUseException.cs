namespace Pacco.Services.Identity.Core.Exceptions
{
    public class EmailInUseException : DomainException
    {
        public override string Code => "email_in_use";
        public string Email { get; }

        public EmailInUseException(string email) : base($"Email {email} is already in use.")
        {
            Email = email;
        }
    }
}