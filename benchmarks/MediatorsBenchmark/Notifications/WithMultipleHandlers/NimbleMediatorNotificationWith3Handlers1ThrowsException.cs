using NimbleMediator.Contracts;

namespace MediatorsBenchmark;

public class NimbleMediatorNotificationWith3Handlers1ThrowsException : INotification
{
    public string Name { get; set; } = "Baran";
}

public class NimbleMediatorNotificationWith3Handlers1ThrowsExceptionHandler1 : INotificationHandler<NimbleMediatorNotificationWith3Handlers1ThrowsException>
{
    public Task HandleAsync(NimbleMediatorNotificationWith3Handlers1ThrowsException notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}

public class NimbleMediatorNotificationWith3Handlers1ThrowsExceptionHandler2 : INotificationHandler<NimbleMediatorNotificationWith3Handlers1ThrowsException>
{
    public Task HandleAsync(NimbleMediatorNotificationWith3Handlers1ThrowsException notification, CancellationToken cancellationToken)
    {
        throw new Exception("Benchmark.");
    }
}

public class NimbleMediatorNotificationWith3Handlers1ThrowsExceptionHandler3 : INotificationHandler<NimbleMediatorNotificationWith3Handlers1ThrowsException>
{
    public Task HandleAsync(NimbleMediatorNotificationWith3Handlers1ThrowsException notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}