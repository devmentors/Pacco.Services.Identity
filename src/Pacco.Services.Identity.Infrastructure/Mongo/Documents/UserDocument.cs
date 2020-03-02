using System;
using System.Collections.Generic;
using Convey.Types;

namespace Pacco.Services.Identity.Infrastructure.Mongo.Documents
{
    internal sealed class UserDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; }
        public IEnumerable<string> Permissions { get; set; }
    }
}