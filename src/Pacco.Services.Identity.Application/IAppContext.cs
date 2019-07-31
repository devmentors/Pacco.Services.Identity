namespace Pacco.Services.Identity.Application
{
    public interface IAppContext
    {
        string RequestId { get; }
        IIdentityContext Identity { get; }
    }
}