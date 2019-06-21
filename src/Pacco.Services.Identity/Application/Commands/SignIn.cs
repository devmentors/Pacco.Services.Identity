using Convey.WebApi.Requests;

namespace Pacco.Services.Identity.Application.Commands
{
    public class SignIn : IRequest
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