using MassTransit;
using TaskManagement.Domain.Interfaces;

namespace TaskManagement.Infrastructure.Messaging
{
    /// <summary>
    /// Реализация публикатора событий.
    /// </summary>
    public sealed class MassTransitEventPublisher : IEventPublisher
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public MassTransitEventPublisher(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        /// <inheritdoc />
        public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
            where TEvent : class
        {
            await _publishEndpoint.Publish(@event, cancellationToken);
        }
    }
}
