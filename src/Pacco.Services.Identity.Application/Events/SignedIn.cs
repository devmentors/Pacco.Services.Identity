using System;
using Convey.CQRS.Events;

namespace Pacco.Services.Identity.Application.Events
{
    [Contract]
    public class SignedIn : IEvent
    {
        public Guid Id { get; }
        public string Role { get; }

        public SignedIn(Guid id, string role)
        {
            Id = id;
            Role = role;
        }
    }
}