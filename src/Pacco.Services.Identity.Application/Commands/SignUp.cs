using System;
using Convey.CQRS.Commands;

namespace Pacco.Services.Identity.Application.Commands
{
    [Contract]
    public class SignUp : ICommand
    {
        public Guid Id { get; }
        public string Email { get; }
        public string Password { get; }
        public string Role { get; }

        public SignUp(Guid id, string email, string password, string role)
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id;
            Email = email;
            Password = password;
            Role = role;
        }
    }
}