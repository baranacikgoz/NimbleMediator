using NimbleMediator.Contracts;

namespace MediatorsBenchmark;

public class NimbleMediatorNotificationWith3Handlers : INotification
{
    public string Name { get; set; } = "Baran";
}

public class NimbleMediatorNotificationWith3HandlersHandler1 : INotificationHandler<NimbleMediatorNotificationWith3Handlers>
{
    public Task HandleAsync(NimbleMediatorNotificationWith3Handlers notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}

public class NimbleMediatorNotificationWith3HandlersHandler2 : INotificationHandler<NimbleMediatorNotificationWith3Handlers>
{
    public Task HandleAsync(NimbleMediatorNotificationWith3Handlers notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}

public class NimbleMediatorNotificationWith3HandlersHandler3 : INotificationHandler<NimbleMediatorNotificationWith3Handlers>
{
    public Task HandleAsync(NimbleMediatorNotificationWith3Handlers notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}

