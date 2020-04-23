namespace Pacco.Services.Identity.Core.Exceptions
{
    public class InvalidRefreshTokenException : DomainException
    {
        public override string Code { get; } = "invalid_refresh_token";
        
        public InvalidRefreshTokenException() : base("Invalid refresh token.")
        {
        }
    }
}