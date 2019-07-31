using Pacco.Services.Identity.Application;

namespace Pacco.Services.Identity.Infrastructure
{
    public interface IAppContextFactory
    {
        IAppContext Create();
    }
}