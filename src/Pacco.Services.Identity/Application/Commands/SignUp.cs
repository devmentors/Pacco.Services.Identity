using System;
using Convey.WebApi.Requests;

namespace Pacco.Services.Identity.Application.Commands
{
    public class SignUp : IRequest
    {
        public Guid Id { get; }
        public string Email { get; }
        public string Password { get; }
        public string Role { get; }

        public SignUp(Guid id, string email, string password, string role)
        {
            Id = id;
            Email = email;
            Password = password;
            Role = role;
        }
    }
}