using Convey.CQRS.Commands;

namespace Pacco.Services.Identity.Application.Commands
{
    [Contract]
    public class SignIn : ICommand
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public SignIn(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}