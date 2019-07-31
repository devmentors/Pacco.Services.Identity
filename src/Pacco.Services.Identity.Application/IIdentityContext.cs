namespace Pacco.Services.Identity.Application
{
    public interface IIdentityContext
    {
        string Id { get; }
        string Role { get; }
        bool IsAuthenticated { get; }
    }
}