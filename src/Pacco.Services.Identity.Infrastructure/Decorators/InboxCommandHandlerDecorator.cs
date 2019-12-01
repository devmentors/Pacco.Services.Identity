using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Convey.MessageBrokers;
using Convey.MessageBrokers.Inbox;

namespace Pacco.Services.Identity.Infrastructure.Decorators
{
    internal sealed class InboxCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand>
        where TCommand : class, ICommand
    {
        private readonly ICommandHandler<TCommand> _handler;
        private readonly IMessageInbox _inbox;
        private readonly IMessagePropertiesAccessor _messagePropertiesAccessor;

        public InboxCommandHandlerDecorator(ICommandHandler<TCommand> handler, IMessageInbox inbox,
            IMessagePropertiesAccessor messagePropertiesAccessor)
        {
            _handler = handler;
            _inbox = inbox;
            _messagePropertiesAccessor = messagePropertiesAccessor;
        }

        public Task HandleAsync(TCommand command)
            => _inbox.TryProcessAsync(_messagePropertiesAccessor.MessageProperties?.MessageId,
                () => _handler.HandleAsync(command));
    }
}