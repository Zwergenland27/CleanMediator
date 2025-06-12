using CleanDomainValidation.Domain;

namespace CleanMediator.Queries;

/// <summary>
/// Extensions for queries
/// </summary>
public static class QueryExtensions
{
    /// <summary>
    /// Asynchronously send a query that can fail to a handler
    /// </summary>
    /// <param name="query">Query object</param>
    /// <param name="cancellationToken">Optional cancellation token</param>
    /// <typeparamref name="TResponse">Response type</typeparamref>
    /// <returns>Task represents the run operation. The returned object contains status information about the query success including the result type</returns>
    public static Task<CanFail<TResponse>> RunAsync<TResponse>(
        this IMediator mediator,
        IQuery<TResponse> query,
        CancellationToken cancellationToken = default)
    {
        return mediator.SendAsync(query, cancellationToken);
    }
}