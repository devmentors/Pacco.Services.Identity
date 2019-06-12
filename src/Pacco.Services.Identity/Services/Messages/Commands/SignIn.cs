using Convey.CQRS.Commands;
using Convey.WebApi.CQRS;
using Convey.WebApi.Requests;

namespace Pacco.Services.Identity.Services.Messages.Commands
{
    [PublicMessage]
    public class SignIn : ICommand, IRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public SignIn()
        {
        }

        public SignIn(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}