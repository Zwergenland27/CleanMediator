using CleanDomainValidation.Domain;

namespace CleanMediator.Commands;

/// <summary>
/// Extensions for commands
/// </summary>
public static class CommandExtensions
{
    /// <summary>
    /// Asynchronously send a command that can fail to a handler
    /// </summary>
    /// <param name="mediator">Mediator</param>
    /// <param name="command">Command object</param>
    /// <param name="cancellationToken">Optional cancellation token</param>
    /// <returns>Task represents the execute operation. The returned object contains status information about the command success</returns>
    public static Task<CanFail> ExecuteAsync(
        this IMediator mediator,
        ICommand command,
        CancellationToken cancellationToken = default)
    {
        return mediator.SendAsync(command, cancellationToken);
    }
    
    /// <summary>
    /// Asynchronously send a command that can fail with a return type to a handler
    /// </summary>
    /// <param name="mediator">Mediator</param>
    /// <param name="command">Command object</param>
    /// <param name="cancellationToken">Optional cancellation token</param>
    /// <typeparamref name="TResponse">Response type</typeparamref>
    /// <returns>Task represents the execute operation. The returned object contains status information about the command success including the result type</returns>
    public static Task<CanFail<TResponse>> ExecuteAsync<TResponse>(
        this IMediator mediator,
        ICommand<TResponse> command,
        CancellationToken cancellationToken = default)
    {
        return mediator.SendAsync(command, cancellationToken);
    }
}