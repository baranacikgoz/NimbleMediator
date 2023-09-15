using MediatR;

namespace MediatorsBenchmark;

public class MediatRNotificationWithMultipleHandlers : INotification
{
    public string Name { get; set; } = "Baran";
}

public class MediatRNotificationWithMultipleHandlersHandler1 : INotificationHandler<MediatRNotificationWithMultipleHandlers>
{
    public Task Handle(MediatRNotificationWithMultipleHandlers notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}

public class MediatRNotificationWithMultipleHandlersHandler2 : INotificationHandler<MediatRNotificationWithMultipleHandlers>
{
    public Task Handle(MediatRNotificationWithMultipleHandlers notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}

public class MediatRNotificationWithMultipleHandlersHandler3 : INotificationHandler<MediatRNotificationWithMultipleHandlers>
{
    public Task Handle(MediatRNotificationWithMultipleHandlers notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}