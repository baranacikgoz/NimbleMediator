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
        var handlers = _serviceProvider.GetServices<INotificationHandler<TNotification>>().ToArray();
        if (!_publisherTypeMappings.TryGetValue(typeof(TNotification), out var publisherType))
        {
            throw new InvalidOperationException($"No publisher type registered for {typeof(TNotification).Name}");
        }

        if (publisherType == NotificationPublisherType.TaskWhenAll)
        {
            Task[] tasks = new Task[handlers.Length];
            for (int i = 0; i < handlers.Length; i++)
            {
                tasks[i] = handlers[i].HandleAsync(notification, cancellationToken);
            }

            await Task.WhenAll(tasks).ConfigureAwait(false);
            return;
        }

        // publisherType is NotificationPublisherType.Foreach here:

        int maxExceptionCount = handlers.Length;
        Exception[] exceptions = new Exception[maxExceptionCount];
        int index = 0;
        bool hasException = false;

        foreach (var handler in handlers)
        {
            try
            {
                await handler.HandleAsync(notification, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                exceptions[index] = ex;
                index++;
                if (!hasException)
                {
                    hasException = true;
                }
            }
        }

        if (hasException)
        {
            throw new AggregateException("One or more notification handlers failed.", exceptions[..index]);
        }
    }
}