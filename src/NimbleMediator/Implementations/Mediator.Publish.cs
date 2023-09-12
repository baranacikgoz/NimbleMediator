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
    /// <exception cref="InvalidOperationException">Thrown when no publisher type is registered for the notification type.</exception>
    /// <exception cref="AggregateException">Thrown when one or more notification handlers fail.</exception>
    public async Task PublishAsync<TNotification>(TNotification notification, CancellationToken cancellationToken)
    where TNotification : INotification
    {
        var handlers = _serviceProvider.GetServices<INotificationHandler<TNotification>>();

        if (!_publisherTypeMappings.TryGetValue(typeof(TNotification), out var publisherType))
        {
            throw new InvalidOperationException($"No publisher type registered for {typeof(TNotification).Name}");
        }

        if (publisherType == NotificationPublisherType.TaskWhenAll)
        {
            var tasks = new List<Task>();
            foreach (var handler in handlers)
            {
                tasks.Add(handler.HandleAsync(notification, cancellationToken));
            }

            await Task.WhenAll(tasks).ConfigureAwait(false);
            return;
        }

        // publisherType is NotificationPublisherType.Foreach here:

        List<Exception>? exceptions = null;

        foreach (var handler in handlers)
        {
            try
            {
                await handler.HandleAsync(notification, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
#pragma warning disable CA1508 // Avoid dead conditional code
                exceptions ??= new List<Exception>();
#pragma warning restore CA1508 // Avoid dead conditional code

                exceptions.Add(ex);
            }
        }

        if (exceptions != null)
        {
            throw new AggregateException("One or more notification handlers failed.", exceptions);
        }
    }
}