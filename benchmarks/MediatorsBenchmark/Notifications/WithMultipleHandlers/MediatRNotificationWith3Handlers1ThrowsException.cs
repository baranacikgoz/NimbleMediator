using MediatR;

namespace MediatorsBenchmark;

public class MediatRNotificationWith3Handlers1ThrowsException : INotification
{
    public string Name { get; set; } = "Baran";
}

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
        throw new Exception("Benchmark.");
    }
}

public class MediatRNotificationWith3Handlers1ThrowsExceptionHandler3 : INotificationHandler<MediatRNotificationWith3Handlers1ThrowsException>
{
    public Task Handle(MediatRNotificationWith3Handlers1ThrowsException notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}





