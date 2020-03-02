using System;
using System.Collections.Generic;
using Pacco.Services.Identity.Application.DTO;

namespace Pacco.Services.Identity.Application.Services
{
    public interface IJwtProvider
    {
        AuthDto Create(Guid userId, string role, string audience = null,
            IDictionary<string, IEnumerable<string>> claims = null);
    }
}