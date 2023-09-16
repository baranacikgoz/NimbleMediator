using NimbleMediator.Contracts;

namespace NimbleMediator.NotificationPublishers;

/// <summary>
/// The robust publisher that does not stop publishing when one or more handlers fail ensuring that all handlers are executed.
/// It is relatively slower and more memory consuming due it's robust nature.
/// </summary>
public class ForeachAwaitRobustPublisher : INotificationPublisher
{
    private const string _aggregateExceptionMessage = "One or more notification handlers failed.";

    /// <summary>
    /// Publishes a notification to all registered handlers.
    /// Does not stop publishing when one or more handlers fail ensuring that all handlers are executed.
    /// </summary>
    /// <typeparam name="TNotification"></typeparam>
    /// <param name="notification"></param>
    /// <param name="handlers"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="AggregateException">Thrown when more than one handler is registered and more than one notification handlers fail.</exception>
    /// <remarks>
    /// If only a single handler is registered and it throws an exception, that exception will be thrown directly, not wrapped in an AggregateException.
    /// </remarks>
    public Task PublishAsync<TNotification>(TNotification notification, IEnumerable<INotificationHandler<TNotification>> handlers, CancellationToken cancellationToken)
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
            return handlersArray[0].HandleAsync(notification, cancellationToken);
        }

        return PublishAsyncInternal(notification, handlersArray, cancellationToken);
    }

    // This method is extracted to avoid call 'await' inside the PublishAsync method,
    // thus preventing the creation of an additional state machine and reducing overhead. 
    // The state machine will only be created when the user calls the PublishAsync method outside of the library.
    // Yes, it seems like even beyond micro-optimization, but makes difference in benchmarks and high-throughput scenarios.
    private static async Task PublishAsyncInternal<TNotification>(TNotification notification, INotificationHandler<TNotification>[] handlersArray, CancellationToken cancellationToken) where TNotification : INotification
    {
        List<Exception>? exceptions = null;

        for (int i = 0; i < handlersArray.Length; i++)
        {
            try
            {
                await handlersArray[i].HandleAsync(notification, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
#pragma warning disable CA1508 // Compiler claims that the exceptions will always be null, but it is not.
                exceptions ??= new List<Exception>(handlersArray.Length);
#pragma warning restore CA1508

                exceptions.Add(ex);
            }
        }

        if (exceptions is not null)
        {
            throw new AggregateException(_aggregateExceptionMessage, exceptions);
        }
    }
}
