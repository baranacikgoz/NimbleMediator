using Microsoft.Extensions.DependencyInjection;
using NimbleMediator.Contracts;

namespace NimbleMediator.Implementations;

public partial class Mediator
{
    /// <summary>
    /// Publishes a notification to all registered handlers.
    /// </summary>
    /// <typeparam name="TNotification"></typeparam>
    /// <param name="notification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="AggregateException">Thrown when one or more notification handlers fail.</exception>
    public async Task PublishAsync<TNotification>(TNotification notification, CancellationToken cancellationToken)
    where TNotification : INotification
    {
        if (!_publisherTypeMappings.TryGetValue(typeof(TNotification), out var publisherType))
        {
            throw new InvalidOperationException($"No publisher type registered for {typeof(TNotification).Name}");
        }

        var handlers = _serviceProvider.GetServices<INotificationHandler<TNotification>>();

        var publisher = (INotificationPublisher)_serviceProvider.GetRequiredService(publisherType);

        await publisher.PublishAsync(notification, handlers, cancellationToken).ConfigureAwait(false);
    }
}