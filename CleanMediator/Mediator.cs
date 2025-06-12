using CleanDomainValidation.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace CleanMediator;

/// <inheritdoc/>
public class Mediator(IServiceProvider serviceProvider) : IMediator
{
    /// <inheritdoc/>
    public Task<CanFail> SendAsync(IRequest request, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(IRequestHandler<>).MakeGenericType(request.GetType());
        dynamic handler = serviceProvider.GetRequiredService(handlerType);
        return handler.Handle((dynamic)request, cancellationToken);
    }
    
    /// <inheritdoc/>
    public Task<CanFail<TResponse>> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
        dynamic handler = serviceProvider.GetRequiredService(handlerType);
        return handler.Handle((dynamic)request, cancellationToken);
    }
}