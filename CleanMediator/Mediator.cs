using CleanDomainValidation.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace CleanMediator;

public class Mediator(IServiceProvider serviceProvider)
{
    public Task<CanFail> SendAsync<TRequest>(TRequest request, CancellationToken cancellationToken = default)
        where TRequest : IRequest
    {
        var handler = serviceProvider.GetRequiredService<IRequestHandler<TRequest>>();
        return handler.Handle(request, cancellationToken);
    }
    
    public Task<CanFail<TResponse>> SendAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default)
        where TRequest : IRequest<TResponse>
    {
        var handler = serviceProvider.GetRequiredService<IRequestHandler<TRequest, TResponse>>();
        return handler.Handle(request, cancellationToken);
    }
}