using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Convey.MessageBrokers;
using Pacco.Services.Identity.Application.Commands;
using Pacco.Services.Identity.Application.DTO;
using Pacco.Services.Identity.Application.Events;
using Pacco.Services.Identity.Application.Events.Rejected;
using Pacco.Services.Identity.Core.Entities;
using Pacco.Services.Identity.Core.Exceptions;
using Pacco.Services.Identity.Core.Repositories;

namespace Pacco.Services.Identity.Application.Services.Identity
{
    public class IdentityService : IIdentityService
    {
        private static readonly Regex EmailRegex = new Regex(
            @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
            @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);

        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly IJwtProvider _jwtProvider;
        private readonly IMessageBroker _messageBroker;
        private readonly ICorrelationContextAccessor _contextAccessor;

        public IdentityService(IUserRepository userRepository, IPasswordService passwordService,
            IJwtProvider jwtProvider, IMessageBroker messageBroker, ICorrelationContextAccessor contextAccessor)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _jwtProvider = jwtProvider;
            _messageBroker = messageBroker;
            _contextAccessor = contextAccessor;
        }

        public async Task<UserDto> GetAsync(Guid id)
        {
            var user = await _userRepository.GetAsync(id);

            return user is null ? null : new UserDto(user);
        }

        public async Task<JwtDto> SignInAsync(SignIn command)
        {
            _contextAccessor.CorrelationContext = CorrelationContext.FromId(Guid.NewGuid());
            if (!EmailRegex.IsMatch(command.Email))
            {
                var exception = new InvalidCredentialsException(command.Email);
                await _messageBroker.PublishAsync(new SignInRejected(command.Email, exception.Message, exception.Code));
                throw exception;
            }

            var user = await _userRepository.GetAsync(command.Email);
            if (user is null || !_passwordService.IsValid(user.Password, command.Password))
            {
                var exception = new InvalidCredentialsException(command.Email);
                await _messageBroker.PublishAsync(new SignInRejected(command.Email, exception.Message, exception.Code));
                throw exception;
            }

            var token = _jwtProvider.Create(user.Id, user.Role);
            await _messageBroker.PublishAsync(new SignedIn(user.Id, user.Role));

            return token;
        }

        public async Task SignUpAsync(SignUp command)
        {
            _contextAccessor.CorrelationContext = CorrelationContext.FromId(Guid.NewGuid());
            if (!EmailRegex.IsMatch(command.Email))
            {
                throw new InvalidEmailException(command.Email);
            }

            var user = await _userRepository.GetAsync(command.Email);
            if (!(user is null))
            {
                var exception = new EmailInUseException(command.Email);
                await _messageBroker.PublishAsync(new SignUpRejected(command.Email, exception.Message, exception.Code));
                throw exception;
            }

            var role = string.IsNullOrWhiteSpace(command.Role) ? "user" : command.Role.ToLowerInvariant();
            var password = _passwordService.Hash(command.Password);
            user = new User(command.Id, command.Email, password, role, DateTime.UtcNow);
            await _userRepository.AddAsync(user);
            await _messageBroker.PublishAsync(new SignedUp(user.Id, user.Email, user.Role));
        }
    }
}