using Mottu.Domain.Common;

namespace Mottu.Application.Interfaces;

public interface IEventPublisher
{
    Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) 
        where TEvent : IDomainEvent;
}

