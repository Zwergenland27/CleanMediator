using CleanDomainValidation.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace CleanMediator;

/// <inheritdoc/>
public class Mediator(IServiceProvider serviceProvider) : IMediator
{
    public IServiceProvider ServiceProvider => serviceProvider;

    /// <inheritdoc/>
    public Task<CanFail> SendAsync(IRequest request, CancellationToken cancellationToken = default)
    {
        return SendAsync(request, serviceProvider, cancellationToken);
    }

    /// <inheritdoc/>
    public Task<CanFail> SendInOwnScopeAsync(IRequest request, CancellationToken cancellationToken = default)
    {
        using var scope = serviceProvider.CreateScope();
        return SendAsync(request, scope.ServiceProvider, cancellationToken);
    }
    
    private static Task<CanFail> SendAsync(
        IRequest request,
        IServiceProvider serviceProvider,
        CancellationToken cancellationToken)
    {
        var handlerType = typeof(IRequestHandler<>).MakeGenericType(request.GetType());
        dynamic handler = serviceProvider.GetRequiredService(handlerType);
        return handler.Handle((dynamic)request, cancellationToken);
    }
    
    /// <inheritdoc/>
    public Task<CanFail<TResponse>> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        return SendAsync(request, serviceProvider, cancellationToken);
    }

    /// <inheritdoc/>
    public Task<CanFail<TResponse>> SendInOwnScopeAsync<TResponse>(IRequest<TResponse> request,
        CancellationToken cancellationToken = default)
    {
        using var scope = serviceProvider.CreateScope();
        return SendAsync(request, scope.ServiceProvider, cancellationToken);
    }
    
    private static Task<CanFail<TResponse>> SendAsync<TResponse>(
        IRequest<TResponse> request,
        IServiceProvider serviceProvider,
        CancellationToken cancellationToken)
    {
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
        dynamic handler = serviceProvider.GetRequiredService(handlerType);
        return handler.Handle((dynamic)request, cancellationToken);
    }
}