using MediatR;

namespace MediatorsBenchmark;

public record MediatRNotificationWith1Handler(string Name) : INotification;

public class MediatRNotificationWith1HandlerHandler : INotificationHandler<MediatRNotificationWith1Handler>
{
    public Task Handle(MediatRNotificationWith1Handler notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}

public record MediatRNotificationWith1HandlerThrowsException(string Name) : INotification;

public class MediatRNotificationWith1HandlerThrowsExceptionHandler : INotificationHandler<MediatRNotificationWith1HandlerThrowsException>
{
    public Task Handle(MediatRNotificationWith1HandlerThrowsException notification, CancellationToken cancellationToken)
    {
        throw new Exception("Benchmark");
    }
}

public record MediatRNotificationWith3Handlers(string Name) : INotification;

public class MediatRNotificationWith3HandlersHandler1 : INotificationHandler<MediatRNotificationWith3Handlers>
{
    public Task Handle(MediatRNotificationWith3Handlers notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}

public class MediatRNotificationWith3HandlersHandler2 : INotificationHandler<MediatRNotificationWith3Handlers>
{
    public Task Handle(MediatRNotificationWith3Handlers notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}

public class MediatRNotificationWith3HandlersHandler3 : INotificationHandler<MediatRNotificationWith3Handlers>
{
    public Task Handle(MediatRNotificationWith3Handlers notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}

public record MediatRNotificationWith3Handlers1ThrowsException(string Name) : INotification;

public class MediatRNotificationWith3Handlers1ThrowsExceptionHandler1 : INotificationHandler<MediatRNotificationWith3Handlers1ThrowsException>
{
    public Task Handle(MediatRNotificationWith3Handlers1ThrowsException notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}

public class MediatRNotificationWith3Handlers1ThrowsExceptionHandler2 : INotificationHandler<MediatRNotificationWith3Handlers1ThrowsException>
{
    public Task Handle(MediatRNotificationWith3Handlers1ThrowsException notification, CancellationToken cancellationToken)
    {
        throw new Exception("Benchmark");
    }
}

public record MediatRNotificationWith3Handlers1ThrowsExceptionHandler3 : INotificationHandler<MediatRNotificationWith3Handlers1ThrowsException>
{
    public Task Handle(MediatRNotificationWith3Handlers1ThrowsException notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}