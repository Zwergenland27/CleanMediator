using CleanDomainValidation.Domain;

namespace CleanMediator;

public interface IMediator
{
    Task<CanFail> SendAsync(IRequest request, CancellationToken cancellationToken = default);

    Task<CanFail<TResponse>> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
}