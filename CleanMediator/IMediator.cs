using CleanDomainValidation.Domain;

namespace CleanMediator;

/// <summary>
/// Mediator that encapsulates the logic of handling requests including dependency injection
/// </summary>
public interface IMediator
{
    /// <summary>
    /// Asynchronously send a request that can fail to a handler
    /// </summary>
    /// <param name="request">Request object</param>
    /// <param name="cancellationToken">Optional cancellation token</param>
    /// <returns>Task represents the send operation. The returned object contains status information about the request success</returns>
    Task<CanFail> SendAsync(IRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously send a request that can fail with a return type to a handler
    /// </summary>
    /// <param name="request">Request object</param>
    /// <param name="cancellationToken">Optional cancellation token</param>
    /// <typeparamref name="TResponse">Response type</typeparamref>
    /// <returns>Task represents the send operation. The returned object contains status information about the request success including the result type</returns>
    Task<CanFail<TResponse>> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
}