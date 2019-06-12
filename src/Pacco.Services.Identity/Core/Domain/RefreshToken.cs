using System;
using Convey.Types;

namespace Pacco.Services.Identity.Core.Domain
{
    public class RefreshToken : IIdentifiable<Guid>
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public string Token { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? RevokedAt { get; private set; }
        public bool Revoked => RevokedAt.HasValue;

        protected RefreshToken()
        {
        }

        public RefreshToken(User user, string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ConveyException(ErrorCodes.InvalidRefreshToken,
                    $"Invalid refresh token'.");
            }

            Id = Guid.NewGuid();
            UserId = user.Id;
            CreatedAt = DateTime.UtcNow;
            Token = token;
        }

        public void Revoke()
        {
            if (Revoked)
            {
                throw new ConveyException(ErrorCodes.RefreshTokenAlreadyRevoked,
                    $"Refresh token: '{Id}' was already revoked at '{RevokedAt}'.");
            }

            RevokedAt = DateTime.UtcNow;
        }
    }
}