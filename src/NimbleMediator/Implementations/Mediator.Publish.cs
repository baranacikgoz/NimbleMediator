using Microsoft.Extensions.DependencyInjection;
using NimbleMediator.Contracts;

namespace NimbleMediator.Implementations;

public partial class Mediator
{
    private const string _aggregateExceptionMessage = "One or more notification handlers failed.";

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
        if (!_publisherTypeMappings.TryGetValue(typeof(TNotification), out NotificationPublisherType publisherType))
        {
            throw new InvalidOperationException($"No publisher type registered for {typeof(TNotification).Name}");
        }

        // I've tried to cast this IEnumarable to various types and turned out luckily it is an array internally.
        // Casted it into array, so we can use the indexer directly and the length.
        var handlers = (INotificationHandler<TNotification>[])_serviceProvider.GetServices<INotificationHandler<TNotification>>();

        if (publisherType == NotificationPublisherType.ForeachAwait)
        {
            await HandlePublishForeachAwait(notification, handlers, cancellationToken).ConfigureAwait(false);
        }
        else
        {
            await HandlePublishTaskWhenAll(notification, handlers, cancellationToken).ConfigureAwait(false);
        }
    }

    private static async Task HandlePublishForeachAwait<TNotification>(TNotification notification, INotificationHandler<TNotification>[] handlers, CancellationToken cancellationToken) where TNotification : INotification
    {
        // If there is only one handler,
        // no need for foreach's overhead.
        if (handlers.Length == 1)
        {
            try
            {
                await handlers[0].HandleAsync(notification, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new AggregateException(_aggregateExceptionMessage, ex);
            }

            return;
        }

        Exception[]? exceptions = null;
        int? index = null;

        foreach (var handler in handlers)
        {
            try
            {
                await handler.HandleAsync(notification, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
#pragma warning disable CA1508 // Compiler claims that the exceptions will always be null, but it is not.
                exceptions ??= new Exception[handlers.Length];
                index ??= 0;
#pragma warning restore CA1508

                exceptions[index.Value] = ex;
                index++;
            }
        }

        if (index.HasValue)
        {
            throw new AggregateException(_aggregateExceptionMessage, OnlyExceptions(exceptions!, index.Value));
        }
    }

    private static async Task HandlePublishTaskWhenAll<TNotification>(TNotification notification, INotificationHandler<TNotification>[] handlers, CancellationToken cancellationToken) where TNotification : INotification
    {
        // If there is only one handler,
        // no need to allocate an array and no need to overhead of Task.WhenAll.
        if (handlers.Length == 1)
        {

            try
            {
                await handlers[0].HandleAsync(notification, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new AggregateException(_aggregateExceptionMessage, ex);
            }

            return;
        }

        // If there are more than one handlers, allocate an array and use Task.WhenAll.
        var tasks = new Task[handlers.Length];

        for (int i = 0; i < handlers.Length; i++)
        {
            tasks[i] = handlers[i].HandleAsync(notification, cancellationToken);
        }

        try
        {
            await Task.WhenAll(tasks).ConfigureAwait(false);
        }
        catch (AggregateException ae)
        {
            // Throwing a new AggregateException from an AggregateException to be sure the same exception message is used across all AggregateException.
            // I don't think thats a good idea, maybe do something else??
            throw new AggregateException(_aggregateExceptionMessage, ae.InnerExceptions);
        }
    }

    private static ICollection<Exception> OnlyExceptions(Exception[] exceptions, int currentIndex)
    {
        // It would be elegant to return a collection only contains exceptions, not null values.
        // Currently the array might contain null values because some handlers might have succeeded
        // while some might have failed.

        if (currentIndex == exceptions.Length)
        {
            // If current exception index is equal to the length of the array,
            // it means all handlers have failed.
            // Therefore, we can return the array as it is (It cannot contain null values, full of exceptions).
            return exceptions;
        }

        // If current exception index is less than the length of the array,
        // it means after the current index, there are some null values in the array.
        // So we have to return a new array that contains only exceptions.

        return exceptions[0..currentIndex]; // Reconsider this logic, maybe there is a more efficient way to do this.
    }
}