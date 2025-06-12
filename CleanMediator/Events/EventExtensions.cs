using CleanDomainValidation.Domain;
using CleanMediator.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace CleanMediator.Events;

/// <summary>
/// Extensions for events
/// </summary>
public static class EventExtensions
{
    /// <summary>
    /// Asynchronously publishes an event that can fail to a handler
    /// </summary>
    /// <param name="event">Event object</param>
    /// <param name="cancellationToken">Optional cancellation token</param>
    /// <returns>Task represents the publish operation. The returned object contains status information about the event success</returns>
    public static Task<CanFail> PublishAsync(
        this IMediator mediator,
        IEvent @event,
        CancellationToken cancellationToken = default)
    {
        return mediator.SendInOwnScopeAsync(@event, cancellationToken);
    }
}