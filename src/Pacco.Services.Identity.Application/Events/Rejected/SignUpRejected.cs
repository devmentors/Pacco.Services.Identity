using Convey.CQRS.Events;

namespace Pacco.Services.Identity.Application.Events.Rejected
{
    [Contract]
    public class SignUpRejected : IRejectedEvent
    {
        public string Email { get; }
        public string Reason { get; }
        public string Code { get; }

        public SignUpRejected(string email, string reason, string code)
        {
            Email = email;
            Reason = reason;
            Code = code;
        }
    }
}