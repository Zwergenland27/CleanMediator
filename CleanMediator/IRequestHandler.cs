using CleanDomainValidation.Domain;

namespace CleanMediator;

/// <summary>
/// Defines handler for request of type <typeparamref name="TRequest"/>
/// </summary>
public interface IRequestHandler<in TRequest>
    where TRequest : IRequest
{
    /// <summary>
    /// Actual request logic
    /// </summary>
    /// <param name="query">Request object</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<CanFail> Handle(TRequest query, CancellationToken cancellationToken);
}

/// <summary>
/// Defines handler for request of type <typeparamref name="TRequest"/> that return <typeparamref name="TResponse"/>
/// </summary>
public interface IRequestHandler<in TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    /// <summary>
    /// Actual request logic
    /// </summary>
    /// <param name="query">Request object</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<CanFail<TResponse>> Handle(TRequest query, CancellationToken cancellationToken);
}