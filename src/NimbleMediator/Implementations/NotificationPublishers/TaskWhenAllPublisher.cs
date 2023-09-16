using NimbleMediator.Contracts;

namespace NimbleMediator.NotificationPublishers;

public class TaskWhenAllPublisher : INotificationPublisher
{
    public Task PublishAsync<TNotification>(TNotification notification, IEnumerable<INotificationHandler<TNotification>> handlers, CancellationToken cancellationToken)
        where TNotification : INotification
    {
        if (handlers is not INotificationHandler<TNotification>[] handlersArray)
        {
            throw new ArgumentException("The default Microsoft DI container should have returned an array of handlers.");
        }

        // If there is only one handler,
        // no need to allocate an array and no need to overhead of Task.WhenAll.
        if (handlersArray.Length == 1)
        {

            return handlersArray[0].HandleAsync(notification, cancellationToken);
        }

        // If there are more than one handlers, allocate an array and use Task.WhenAll.
        var tasks = new Task[handlersArray.Length];

        for (int i = 0; i < handlersArray.Length; i++)
        {
            tasks[i] = handlersArray[i].HandleAsync(notification, cancellationToken);
        }

        return Task.WhenAll(tasks);
    }
}
