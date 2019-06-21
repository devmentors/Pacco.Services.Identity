using System;
using System.Text.RegularExpressions;
using Convey.Types;
using Pacco.Services.Identity.Core.Exceptions;

namespace Pacco.Services.Identity.Core.Entities
{
    public class User : IIdentifiable<Guid>
    {
        private static readonly Regex EmailRegex = new Regex(
            @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
            @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);

        public Guid Id { get; private set; }
        public string Email { get; private set; }
        public string Role { get; private set; }
        public string Password { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        protected User()
        {
        }

        public User(Guid id, string email, string password, string role)
        {
            if (!EmailRegex.IsMatch(email))
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
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}