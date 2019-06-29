using System.Threading.Tasks;
using Convey.CQRS.Events;
using Convey.MessageBrokers;
using Pacco.Services.Identity.Application.Services;

namespace Pacco.Services.Identity.Infrastructure.MessageBrokers
{
    internal class MessageBroker : IMessageBroker
    {
        private readonly IBusPublisher _busPublisher;
        private readonly ICorrelationContextAccessor _contextAccessor;

        public MessageBroker(IBusPublisher busPublisher, ICorrelationContextAccessor contextAccessor)
        {
            _busPublisher = busPublisher;
            _contextAccessor = contextAccessor;
        }

        public Task PublishAsync<T>(T @event) where T : class, IEvent
            => _busPublisher.PublishAsync(@event, _contextAccessor.CorrelationContext);
    }
}