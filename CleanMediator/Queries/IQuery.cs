namespace CleanMediator.Queries;

/// <summary>
/// Marker interface for queries with return type <typeparamref name="TResponse"/>
/// </summary>
public interface IQuery<TResponse> : IRequest<TResponse>;