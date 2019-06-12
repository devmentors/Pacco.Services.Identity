using System;
using Convey.CQRS.Events;
using Convey.WebApi.CQRS;

namespace Pacco.Services.Identity.Services.Messages.Events
{
    [PublicMessage]
    public class SignedUp : IEvent
    {
        public Guid UserId { get; }
        public string Role { get; }
        
        public SignedUp(Guid userId, string role)
        {
            UserId = userId;
            Role = role;
        }
    }
}