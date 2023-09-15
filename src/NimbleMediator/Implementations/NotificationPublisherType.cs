namespace NimbleMediator.Implementations;

/// <summary>
/// Defines the type of notification publisher.
/// </summary>
public enum NotificationPublisherType
{
    /// <summary>
    /// Publishes notifications by sequentially awaiting with a loop.
    /// </summary>
    ForeachAwait,

    /// <summary>
    /// Publishes notifications by awaiting with Task.WhenAll().
    /// </summary>
    TaskWhenAll
}
