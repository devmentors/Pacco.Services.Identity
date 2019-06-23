using System.Threading.Tasks;
using Convey.CQRS.Events;

namespace Pacco.Services.Identity.Application.Services
{
    public interface IMessageBroker
    {
        Task PublishAsync<T>(T @event) where T : class, IEvent;
    }
}