using System;
using Convey.CQRS.Commands;

namespace Pacco.Services.Identity.Application.Commands
{
    [Contract]
    public class SignUp : ICommand
    {
        public Guid UserId { get; }
        public string Email { get; }
        public string Password { get; }
        public string Role { get; }

        public SignUp(Guid userId, string email, string password, string role)
        {
            UserId = userId == Guid.Empty ? Guid.NewGuid() : userId;
            Email = email;
            Password = password;
            Role = role;
        }
    }
}