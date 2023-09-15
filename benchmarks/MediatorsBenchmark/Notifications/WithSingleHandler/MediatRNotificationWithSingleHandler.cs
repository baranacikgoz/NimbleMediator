using MediatR;

namespace MediatorsBenchmark;

public class MediatRNotificationWithSingleHandler : INotification
{
    public string Name { get; set; } = "Baran";
}

public class MediatRNotificationWithSingleHandlerHandler : INotificationHandler<MediatRNotificationWithSingleHandler>
{
    public Task Handle(MediatRNotificationWithSingleHandler notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
