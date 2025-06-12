namespace CleanMediator;

/// <summary>
/// Marker interface for any request without return type
/// </summary>
public interface IRequest : CleanDomainValidation.Application.IRequest;

/// <summary>
/// Marker interface for any request with return type <typeparamref name="TResponse"/>
/// </summary>
public interface IRequest<TResponse> : CleanDomainValidation.Application.IRequest;