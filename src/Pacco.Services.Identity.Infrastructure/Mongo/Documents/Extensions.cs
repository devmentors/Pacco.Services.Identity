using Pacco.Services.Identity.Application.DTO;
using Pacco.Services.Identity.Core.Entities;

namespace Pacco.Services.Identity.Infrastructure.Mongo.Documents
{
    public static class Extensions
    {
        public static User AsEntity(this UserDocument document)
            => new User(document.Id, document.Email, document.Password, document.Role, document.CreatedAt);

        public static UserDocument AsDocument(this User entity)
            => new UserDocument
            {
                Id = entity.Id,
                Email = entity.Email,
                Password = entity.Password,
                Role = entity.Role,
                CreatedAt = entity.CreatedAt
            };

        public static UserDto AsDto(this UserDocument document)
            => new UserDto
            {
                Id = document.Id,
                Email = document.Email,
                Role = document.Role,
                CreatedAt = document.CreatedAt
            };
    }
}