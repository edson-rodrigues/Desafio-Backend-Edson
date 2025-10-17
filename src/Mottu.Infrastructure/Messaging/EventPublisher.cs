using MassTransit;
using Mottu.Application.Interfaces;
using Mottu.Domain.Common;

namespace Mottu.Infrastructure.Messaging;

public class EventPublisher : IEventPublisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    public EventPublisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) 
        where TEvent : IDomainEvent
    {
        await _publishEndpoint.Publish(@event, cancellationToken);
    }
}

