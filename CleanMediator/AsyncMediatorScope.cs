using CleanDomainValidation.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace CleanMediator;

/// <summary>
/// Mediator in separate scope that encapsulates the logic of handling requests including dependency injection
/// </summary>
public class AsyncMediatorScope(AsyncServiceScope serviceScope): IMediator, IAsyncDisposable
{
    /// <inheritdoc/>
    public Task<CanFail> SendAsync(IRequest request, CancellationToken cancellationToken = default)
    {
        var mediator = serviceScope.ServiceProvider.GetRequiredService<IMediator>();
        return mediator.SendAsync(request, cancellationToken);
    }

    /// <inheritdoc/>
    public Task<CanFail<TResponse>> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var mediator = serviceScope.ServiceProvider.GetRequiredService<IMediator>();
        return mediator.SendAsync(request, cancellationToken);
    }

    /// <inheritdoc/>
    public ValueTask DisposeAsync()
    {
        return serviceScope.DisposeAsync();
    }
}