using System;

namespace Pacco.Services.Identity.Application.Commands
{
    public class SignUp
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