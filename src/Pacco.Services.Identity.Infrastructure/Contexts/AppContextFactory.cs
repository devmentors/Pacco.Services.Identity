using Convey.MessageBrokers;
using Microsoft.AspNetCore.Http;
using Pacco.Services.Identity.Application;

namespace Pacco.Services.Identity.Infrastructure.Contexts
{
    internal sealed class AppContextFactory : IAppContextFactory
    {
        private static readonly AppContext EmptyContext = new AppContext();
        private readonly ICorrelationContextAccessor _contextAccessor;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AppContextFactory(ICorrelationContextAccessor contextAccessor, IHttpContextAccessor httpContextAccessor)
        {
            _contextAccessor = contextAccessor;
            _httpContextAccessor = httpContextAccessor;
        }

        public IAppContext Create()
        {
            if (!(_contextAccessor.CorrelationContext is null))
            {
                return new AppContext(_contextAccessor.CorrelationContext as CorrelationContext);
            }

            var context = _httpContextAccessor.GetCorrelationContext();
            
            return context is null ? EmptyContext : new AppContext(context);
        }
    }
}