
using CleanDomainValidation.Domain;

namespace CleanMediator.Events;

/// <summary>
/// Defines handler for events of type <typeparamref name="TEvent"/>
/// </summary>
public abstract class EventHandlerBase<TEvent> : IRequestHandler<TEvent>
    where TEvent : IEvent
{
    /// <summary>
    /// Actual event logic
    /// </summary>
    /// <param name="event">Event object</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public abstract Task<CanFail> Handle(TEvent @event, CancellationToken cancellationToken);
}