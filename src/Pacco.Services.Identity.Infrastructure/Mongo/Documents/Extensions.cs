using System.Linq;
using Pacco.Services.Identity.Application.DTO;
using Pacco.Services.Identity.Core.Entities;

namespace Pacco.Services.Identity.Infrastructure.Mongo.Documents
{
    internal static class Extensions
    {
        public static User AsEntity(this UserDocument document)
            => new User(document.Id, document.Email, document.Password, document.Role, document.CreatedAt,
                document.Permissions);

        public static UserDocument AsDocument(this User entity)
            => new UserDocument
            {
                Id = entity.Id,
                Email = entity.Email,
                Password = entity.Password,
                Role = entity.Role,
                CreatedAt = entity.CreatedAt,
                Permissions = entity.Permissions ?? Enumerable.Empty<string>()
            };

        public static UserDto AsDto(this UserDocument document)
            => new UserDto
            {
                Id = document.Id,
                Email = document.Email,
                Role = document.Role,
                CreatedAt = document.CreatedAt,
                Permissions = document.Permissions ?? Enumerable.Empty<string>()
            };

        public static RefreshToken AsEntity(this RefreshTokenDocument document)
            => new RefreshToken(document.Id, document.UserId, document.Token, document.CreatedAt, document.RevokedAt);
        
        public static RefreshTokenDocument AsDocument(this RefreshToken entity)
            => new RefreshTokenDocument
            {
                Id = entity.Id,
                UserId = entity.UserId,
                Token = entity.Token,
                CreatedAt = entity.CreatedAt,
                RevokedAt = entity.RevokedAt
            };
    }
}