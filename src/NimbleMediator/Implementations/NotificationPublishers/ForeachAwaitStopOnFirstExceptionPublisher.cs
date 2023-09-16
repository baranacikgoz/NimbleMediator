using NimbleMediator.Contracts;

namespace NimbleMediator.NotificationPublishers;

/// <summary>
/// The publisher that stops on first exception, not guarantees that all handlers are executed.
/// If you want to ensure that all handlers are executed, use <see cref="ForeachAwaitRobustPublisher"/>.
/// It is relatively faster and less memory consuming due it's fail-fast nature.
/// </summary>
public class ForeachAwaitStopOnFirstExceptionPublisher : INotificationPublisher
{
    /// <summary>
    /// Publishes a notification to all registered handlers.
    /// Stops on first exception, not guarantees that all handlers are executed.
    /// </summary>
    /// <typeparam name="TNotification"></typeparam>
    /// <param name="notification"></param>
    /// <param name="handlers"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <remarks>
    /// May throw an exception if an handler throws an exception.
    /// Does not wraps the exception with a different type of exception, throws directly.
    /// </remarks>
    public async Task PublishAsync<TNotification>(TNotification notification, IEnumerable<INotificationHandler<TNotification>> handlers, CancellationToken cancellationToken)
        where TNotification : INotification
    {
        if (handlers is not INotificationHandler<TNotification>[] handlersArray)
        {
            throw new ArgumentException("The default Microsoft DI container should have returned an array of handlers.");
        }

        // If there is only one handler,
        // no need for a loop's overhead.
        if (handlersArray.Length == 1)
        {
            await handlersArray[0].HandleAsync(notification, cancellationToken).ConfigureAwait(false);

            return;
        }

        for (int i = 0; i < handlersArray.Length; i++)
        {
            await handlersArray[i].HandleAsync(notification, cancellationToken).ConfigureAwait(false);
        }
    }
}
