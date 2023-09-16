using NimbleMediator.Contracts;

namespace NimbleMediator;

public interface INotificationPublisher
{
    Task PublishAsync<TNotification>(TNotification notification, IEnumerable<INotificationHandler<TNotification>> handlers, CancellationToken cancellationToken)
        where TNotification : INotification;
}