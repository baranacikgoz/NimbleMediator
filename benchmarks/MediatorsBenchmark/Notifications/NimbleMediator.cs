using NimbleMediator.Contracts;

namespace MediatorsBenchmark;

public record NimbleMediatorNotificationWith1Handler(string Name) : INotification;

public class NimbleMediatorNotificationWith1HandlerHandler : INotificationHandler<NimbleMediatorNotificationWith1Handler>
{
    public Task HandleAsync(NimbleMediatorNotificationWith1Handler notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}

public record NimbleMediatorNotificationWith1HandlerThrowsException(string Name) : INotification;

public class NimbleMediatorNotificationWith1HandlerThrowsExceptionHandler : INotificationHandler<NimbleMediatorNotificationWith1HandlerThrowsException>
{
    public Task HandleAsync(NimbleMediatorNotificationWith1HandlerThrowsException notification, CancellationToken cancellationToken)
    {
        throw new Exception("Benchmark");
    }
}

public record NimbleMediatorNotificationWith3Handlers(string Name) : INotification;

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

public record NimbleMediatorNotificationWith3Handlers1ThrowsException(string Name) : INotification;

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
        throw new Exception("Benchmark");
    }
}

public class NimbleMediatorNotificationWith3Handlers1ThrowsExceptionHandler3 : INotificationHandler<NimbleMediatorNotificationWith3Handlers1ThrowsException>
{
    public Task HandleAsync(NimbleMediatorNotificationWith3Handlers1ThrowsException notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}