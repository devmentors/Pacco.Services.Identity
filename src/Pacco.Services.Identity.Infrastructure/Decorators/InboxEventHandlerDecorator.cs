using System.Threading.Tasks;
using Convey.CQRS.Events;
using Convey.MessageBrokers;
using Convey.MessageBrokers.Inbox;

namespace Pacco.Services.Identity.Infrastructure.Decorators
{
    internal sealed class InboxEventHandlerDecorator<TEvent> : IEventHandler<TEvent>
        where TEvent : class, IEvent
    {
        private readonly IEventHandler<TEvent> _handler;
        private readonly IMessageInbox _inbox;
        private readonly IMessagePropertiesAccessor _messagePropertiesAccessor;

        public InboxEventHandlerDecorator(IEventHandler<TEvent> handler, IMessageInbox inbox,
            IMessagePropertiesAccessor messagePropertiesAccessor)
        {
            _handler = handler;
            _inbox = inbox;
            _messagePropertiesAccessor = messagePropertiesAccessor;
        }

        public Task HandleAsync(TEvent @event)
            => _inbox.TryProcessAsync(_messagePropertiesAccessor.MessageProperties?.MessageId,
                () => _handler.HandleAsync(@event));
    }
}