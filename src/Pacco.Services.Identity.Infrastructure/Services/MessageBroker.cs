using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Convey.CQRS.Events;
using Convey.MessageBrokers;
using Convey.MessageBrokers.RabbitMQ;
using Microsoft.AspNetCore.Http;
using OpenTracing;
using Pacco.Services.Identity.Application.Services;

namespace Pacco.Services.Identity.Infrastructure.Services
{
    internal sealed class MessageBroker : IMessageBroker
    {
        private readonly IBusPublisher _busPublisher;
        private readonly ICorrelationContextAccessor _contextAccessor;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMessagePropertiesAccessor _messagePropertiesAccessor;
        private readonly ITracer _tracer;
        private readonly string _spanContextHeader;

        public MessageBroker(IBusPublisher busPublisher, ICorrelationContextAccessor contextAccessor,
            IHttpContextAccessor httpContextAccessor, IMessagePropertiesAccessor messagePropertiesAccessor,
            RabbitMqOptions options, ITracer tracer)
        {
            _busPublisher = busPublisher;
            _contextAccessor = contextAccessor;
            _httpContextAccessor = httpContextAccessor;
            _messagePropertiesAccessor = messagePropertiesAccessor;
            _tracer = tracer;
            _spanContextHeader = string.IsNullOrWhiteSpace(options.SpanContextHeader)
                ? "span_context"
                : options.SpanContextHeader;
        }

        public async Task PublishAsync(params IEvent[] events)
        {
            if (events is null || !events.Any())
            {
                return;
            }

            var correlationContext = _contextAccessor.CorrelationContext ??
                                     _httpContextAccessor.GetCorrelationContext();

            var messageProperties = _messagePropertiesAccessor.MessageProperties;
            var correlationId = _messagePropertiesAccessor.MessageProperties?.CorrelationId;
            var spanContext = string.Empty;
            
            if (!(messageProperties is null) && messageProperties.Headers.TryGetValue(_spanContextHeader, out var span)
                                             && span is byte[] spanBytes)
            {
                spanContext = Encoding.UTF8.GetString(spanBytes);
            }

            if (string.IsNullOrWhiteSpace(spanContext))
            {
                spanContext = _tracer.ActiveSpan is null ? string.Empty : _tracer.ActiveSpan.Context.ToString();
            }

            foreach (var @event in events)
            {
                if (@event is null)
                {
                    continue;
                }

                await _busPublisher.PublishAsync(@event, correlationId: correlationId, spanContext: spanContext,
                    messageContext: correlationContext);
            }
        }
    }
}