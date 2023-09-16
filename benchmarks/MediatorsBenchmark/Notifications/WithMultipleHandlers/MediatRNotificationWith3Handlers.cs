using MediatR;

namespace MediatorsBenchmark;

public class MediatRNotificationWith3Handlers : INotification
{
    public string Name { get; set; } = "Baran";
}

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