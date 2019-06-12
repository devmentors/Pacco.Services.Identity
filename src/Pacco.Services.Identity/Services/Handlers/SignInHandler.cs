using System.Threading.Tasks;
using Convey.WebApi.Requests;
using Pacco.Services.Identity.Services.Messages.Commands;

namespace Pacco.Services.Identity.Services.Handlers
{
    public class SignInHandler : IRequestHandler<SignIn, string>
    {
        public SignInHandler()
        {
        }

        public async Task<string> HandleAsync(SignIn request)
        {
            await Task.CompletedTask;
            return "jwt";
        }
    }
}