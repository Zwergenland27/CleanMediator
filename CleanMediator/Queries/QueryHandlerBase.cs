
using CleanDomainValidation.Domain;

namespace CleanMediator.Queries;

/// <summary>
/// Defines handler for queries of type <typeparamref name="TQuery"/> that return <typeparamref name="TResponse"/>
/// </summary>
public abstract class QueryHandlerBase<TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
    /// <summary>
    /// Actual query logic
    /// </summary>
    /// <param name="query">Query object</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public abstract Task<CanFail<TResponse>> Handle(TQuery query, CancellationToken cancellationToken);
}