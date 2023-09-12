namespace NimbleMediator.Contracts;

/// <summary>
/// Defines a request sender.
/// </summary>
public interface ISender
{
    /// <summary>
    /// Sends the request.
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    ValueTask SendAsync<TRequest>(TRequest request, CancellationToken cancellationToken)
        where TRequest : IRequest;

    /// <summary>
    /// Sends the request.
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    ValueTask<TResponse> SendAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken)
        where TRequest : IRequest<TResponse>
        where TResponse : notnull;
}
