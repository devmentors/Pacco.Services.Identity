namespace Pacco.Services.Identity.Core.Exceptions
{
    public class InvalidCredentialsException : DomainException
    {
        public override string Code { get; } = "invalid_credentials";
        public string Email { get; }

        public InvalidCredentialsException(string email) : base("Invalid credentials.")
        {
            Email = email;
        }
    }
}