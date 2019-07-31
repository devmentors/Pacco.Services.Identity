using Pacco.Services.Identity.Application;

namespace Pacco.Services.Identity.Infrastructure.Contexts
{
    public class AppContext : IAppContext
    {
        public string RequestId { get; }
        public IIdentityContext Identity { get; }

        internal AppContext()
        {
            Identity = new IdentityContext();
        }

        public AppContext(CorrelationContext context)
        {
            RequestId = context.CorrelationId;
            Identity = new IdentityContext(context.User);
        }

        public AppContext(string requestId, IIdentityContext identity)
        {
            RequestId = requestId;
            Identity = identity;
        }
    }
}