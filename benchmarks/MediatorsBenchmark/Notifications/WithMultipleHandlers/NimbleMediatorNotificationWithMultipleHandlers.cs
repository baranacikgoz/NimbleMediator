using NimbleMediator.Contracts;

namespace MediatorsBenchmark;

public class NimbleMediatorNotificationWithMultipleHandlers : INotification
{
    public string Name { get; set; } = "Baran";
}

public class NimbleMediatorNotificationWithMultipleHandlersHandler1 : INotificationHandler<NimbleMediatorNotificationWithMultipleHandlers>
{
    public Task HandleAsync(NimbleMediatorNotificationWithMultipleHandlers notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}

public class NimbleMediatorNotificationWithMultipleHandlersHandler2 : INotificationHandler<NimbleMediatorNotificationWithMultipleHandlers>
{
    public Task HandleAsync(NimbleMediatorNotificationWithMultipleHandlers notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}

public class NimbleMediatorNotificationWithMultipleHandlersHandler3 : INotificationHandler<NimbleMediatorNotificationWithMultipleHandlers>
{
    public Task HandleAsync(NimbleMediatorNotificationWithMultipleHandlers notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}

