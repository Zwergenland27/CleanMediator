using Microsoft.Extensions.DependencyInjection;

namespace CleanMediator;

/// <summary>
/// Mediator that encapsulates the logic of handling requests including dependency injection
/// </summary>
public static class ServiceProviderExtensions
{
    /// <summary>
    /// Creates a new <see cref="AsyncMediatorScope"/> that can be used to handle
    /// request handlers which inject scoped services using mediator
    /// </summary>
    public static AsyncMediatorScope CreateAsyncMediatorScope(this IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateAsyncScope();
        return new AsyncMediatorScope(scope);
    }
}