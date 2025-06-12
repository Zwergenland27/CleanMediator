using CleanDomainValidation.Domain;

namespace CleanMediator;

/// <summary>
/// Handler of <typeparamref name="TRequest"/>
/// </summary>
public interface IRequestHandler<in TRequest>
    where TRequest : IRequest
{
    Task<CanFail> Handle(TRequest request, CancellationToken cancellationToken);
}

/// <summary>
/// Handler of <typeparamref name="TRequest"/> with return type <typeparamref name="TResponse"/>
/// </summary>
public interface IRequestHandler<in TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    Task<CanFail<TResponse>> Handle(TRequest request, CancellationToken cancellationToken);
}