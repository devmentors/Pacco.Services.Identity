using System;
using Pacco.Services.Identity.Application.DTO;

namespace Pacco.Services.Identity.Application.Services
{
    public interface IJwtProvider
    {
        JwtDto Create(Guid userId, string role);
    }
}