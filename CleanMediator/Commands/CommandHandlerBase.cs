
using CleanDomainValidation.Domain;

namespace CleanMediator.Commands;

/// <summary>
/// Defines handler for commands of type <typeparamref name="TCommand"/>
/// </summary>
public abstract class CommandHandlerBase<TCommand> : IRequestHandler<TCommand>
    where TCommand : ICommand
{
    /// <summary>
    /// Actual command logic
    /// </summary>
    /// <param name="command">Command object</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public abstract Task<CanFail> Handle(TCommand command, CancellationToken cancellationToken);
}

/// <summary>
/// Defines handler for commands of type <typeparamref name="TCommand"/> that return <typeparamref name="TResponse"/>
/// </summary>
public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    /// <summary>
    /// Actual command logic
    /// </summary>
    /// <param name="command">Command object</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public new Task<CanFail<TResponse>> Handle(TCommand command, CancellationToken cancellationToken);
}