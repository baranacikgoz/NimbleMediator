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
    public Task PublishAsync<TNotification>(TNotification notification, IEnumerable<INotificationHandler<TNotification>> handlers, CancellationToken cancellationToken)
     where TNotification : INotification
    {
        if (handlers is not INotificationHandler<TNotification>[] handlersArray)
        {
            throw new ArgumentException("The default Microsoft DI container should have returned an array of handlers.");
        }

        // If there is only one handler, no need for a loop's overhead.
        if (handlersArray.Length == 1)
        {
            return handlersArray[0].HandleAsync(notification, cancellationToken);
        }

        return PublishAsyncInternal(notification, handlersArray, cancellationToken);
    }

    // This method is extracted to avoid call 'await' inside the PublishAsync method,
    // thus preventing the creation of an additional state machine and reducing overhead. 
    // The state machine will only be created when the user calls the PublishAsync method outside of the library.
    // Yes, it seems like even beyond micro-optimization, but makes difference in benchmarks and high-throughput scenarios.
    private static async Task PublishAsyncInternal<TNotification>(TNotification notification, INotificationHandler<TNotification>[] handlersArray, CancellationToken cancellationToken)
        where TNotification : INotification
    {
        for (int i = 0; i < handlersArray.Length; i++)
        {
            await handlersArray[i].HandleAsync(notification, cancellationToken).ConfigureAwait(false);
        }
    }
}
