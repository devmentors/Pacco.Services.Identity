using System;
using Convey.CQRS.Queries;
using Pacco.Services.Identity.Application.DTO;

namespace Pacco.Services.Identity.Application.Queries
{
    public class GetUser : IQuery<UserDto>
    {
        public Guid UserId { get; set; }
    }
}