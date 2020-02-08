using System;
using System.Threading.Tasks;
using Pacco.Services.Identity.Application.Commands;
using Pacco.Services.Identity.Application.DTO;

namespace Pacco.Services.Identity.Application.Services
{
    public interface IIdentityService
    {
        Task<UserDto> GetAsync(Guid id);
        Task<AuthDto> SignInAsync(SignIn command);
        Task SignUpAsync(SignUp command);
    }
}