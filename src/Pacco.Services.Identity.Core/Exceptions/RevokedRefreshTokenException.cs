namespace Pacco.Services.Identity.Core.Exceptions
{
    public class RevokedRefreshTokenException : DomainException
    {
        public override string Code { get; } = "revoked_refresh_token";
        
        public RevokedRefreshTokenException() : base("Revoked refresh token.")
        {
        }
    }
}