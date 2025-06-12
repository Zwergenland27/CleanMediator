namespace CleanMediator.Commands;

/// <summary>
/// Marker interface for commands without return type
/// </summary>
public interface ICommand : IRequest;


/// <summary>
/// Marker interface for commands with return type <typeparamref name="TResponse"/>
/// </summary>
public interface ICommand<TResponse> : IRequest<TResponse>;