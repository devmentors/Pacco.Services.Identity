using System;
using Convey.Auth;
using Pacco.Services.Identity.Application.DTO;
using Pacco.Services.Identity.Application.Services;

namespace Pacco.Services.Identity.Infrastructure.Auth
{
    public class JwtProvider : IJwtProvider
    {
        private readonly IJwtHandler _jwtHandler;

        public JwtProvider(IJwtHandler jwtHandler)
        {
            _jwtHandler = jwtHandler;
        }

        public JwtDto Create(Guid userId, string role)
        {
            var jwt = _jwtHandler.CreateToken(userId.ToString("N"), role);

            return new JwtDto
            {
                AccessToken = jwt.AccessToken,
                Role = jwt.Role,
                Expires = jwt.Expires
            };
        }
    }
}