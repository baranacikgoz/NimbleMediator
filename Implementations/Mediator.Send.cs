using Microsoft.Extensions.DependencyInjection;
using NimbleMediator.Contracts;

namespace NimbleMediator.Implementations;

public partial class Mediator
{
    /// <summary>
    /// Sends a request to the registered handler.
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException">Thrown when no handler is registered for the request type.</exception>
    public ValueTask SendAsync<TRequest>(TRequest request, CancellationToken cancellationToken)
        where TRequest : IRequest
    {
        if (!_requestsAndHandlers.TryGetValue(typeof(TRequest), out var handlerType))
        {
            throw new InvalidOperationException($"No handler registered for {typeof(TRequest).Name}");
        }

        var service = _serviceProvider.GetRequiredService(handlerType); // maybe do type check here?

        if (service is not IRequestHandler<TRequest> handler)
        {
            throw new InvalidOperationException($"Handler for {typeof(TRequest).Name} does not implement IRequestHandler<{typeof(TRequest).Name}>");
        }

        return handler.HandleAsync(request, cancellationToken);
    }

    /// <summary>
    /// Sends a request to the registered handler and returns the response.
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException">Thrown when no handler is registered for the request type.</exception>
    public ValueTask<TResponse> SendAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken)
        where TRequest : IRequest<TResponse>
        where TResponse : notnull
    {
        if (!_requestsAndHandlers.TryGetValue(typeof(TRequest), out var handlerType))
        {
            throw new InvalidOperationException($"No handler registered for {typeof(TRequest).Name}");
        }

        var service = _serviceProvider.GetRequiredService(handlerType); // maybe do type check here?

        if (service is not IRequestHandler<TRequest, TResponse> handler)
        {
            throw new InvalidOperationException($"Handler for {typeof(TRequest).Name} does not implement IRequestHandler<{typeof(TRequest).Name}, {typeof(TResponse).Name}>");
        }

        return handler.HandleAsync(request, cancellationToken);
    }
}