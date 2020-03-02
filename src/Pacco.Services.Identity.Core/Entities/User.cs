using System;
using System.Collections.Generic;
using System.Linq;
using Pacco.Services.Identity.Core.Exceptions;

namespace Pacco.Services.Identity.Core.Entities
{
    public class User : AggregateRoot
    {
        public string Email { get; private set; }
        public string Role { get; private set; }
        public string Password { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public IEnumerable<string> Permissions { get; private set; }

        public User(Guid id, string email, string password, string role, DateTime createdAt,
            IEnumerable<string> permissions = null)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new InvalidEmailException(email);
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new InvalidPasswordException();
            }

            if (!Entities.Role.IsValid(role))
            {
                throw new InvalidRoleException(role);
            }

            Id = id;
            Email = email.ToLowerInvariant();
            Password = password;
            Role = role.ToLowerInvariant();
            CreatedAt = createdAt;
            Permissions = permissions ?? Enumerable.Empty<string>();
        }
    }
}