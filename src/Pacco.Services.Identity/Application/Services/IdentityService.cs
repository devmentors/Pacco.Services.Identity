using System;
using System.Threading.Tasks;
using Convey.Auth;
using Pacco.Services.Identity.Application.Commands;
using Pacco.Services.Identity.Application.DTO;
using Pacco.Services.Identity.Application.Events;
using Pacco.Services.Identity.Core.Entities;
using Pacco.Services.Identity.Core.Exceptions;
using Pacco.Services.Identity.Core.Repositories;
using Pacco.Services.Identity.Core.Services;

namespace Pacco.Services.Identity.Application.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly IJwtHandler _jwtHandler;
        private readonly IMessageBroker _messageBroker;

        public IdentityService(IUserRepository userRepository, IPasswordService passwordService,
            IJwtHandler jwtHandler, IMessageBroker messageBroker)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _jwtHandler = jwtHandler;
            _messageBroker = messageBroker;
        }

        public async Task<UserDto> GetAsync(Guid id)
        {
            var user = await _userRepository.GetAsync(id);

            return user is null ? null : new UserDto(user);
        }

        public async Task<JsonWebToken> SignInAsync(SignIn command)
        {
            var user = await _userRepository.GetAsync(command.Email);
            if (user is null || !_passwordService.IsValid(user.Password, command.Password))
            {
                throw new InvalidCredentialsException();
            }

            var token = _jwtHandler.CreateToken(user.Id.ToString("N"), user.Role);
            await _messageBroker.PublishAsync(new SignedIn(user.Id, user.Role));

            return token;
        }

        public async Task SignUpAsync(SignUp command)
        {
            var user = await _userRepository.GetAsync(command.Email);
            if (!(user is null))
            {
                throw new EmailInUseException(command.Email);
            }

            var role = string.IsNullOrWhiteSpace(command.Role) ? "user" : command.Role.ToLowerInvariant();
            user = new User(command.Id, command.Email, _passwordService.Hash(command.Password), role);
            await _userRepository.AddAsync(user);
            await _messageBroker.PublishAsync(new SignedUp(user.Id, user.Role));
        }
    }
}