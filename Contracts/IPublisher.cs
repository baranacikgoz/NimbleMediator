using NimbleMediator.Contracts;

namespace NimbleMediator;

/// <summary>
/// Defines an event publisher.
/// </summary>
public interface IPublisher
{
    /// <summary>
    /// Publishes the notification.
    /// </summary>
    /// <typeparam name="TNotification"></typeparam>
    /// <param name="notification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task PublishAsync<TNotification>(TNotification notification, CancellationToken cancellationToken)
        where TNotification : INotification;
}
