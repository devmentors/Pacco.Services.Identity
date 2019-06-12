using System;
using Convey.CQRS.Commands;
using Convey.WebApi.CQRS;

namespace Pacco.Services.Identity.Services.Messages.Commands
{
    [PublicMessage]
    public class SignUp : ICommand
    {
        public Guid UserId { get; }
        public string Email { get; }
        public string Password { get; }
        public string Role { get; }

        public SignUp(Guid userId, string email, string password, string role)
        {
            UserId = userId;
            Email = email;
            Password = password;
            Role = role;
        }
    }
}