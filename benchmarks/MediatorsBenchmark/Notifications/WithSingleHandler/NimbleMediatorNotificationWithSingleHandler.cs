using NimbleMediator.Contracts;

namespace MediatorsBenchmark;

public class NimbleMediatorNotificationWithSingleHandler : INotification
{
    public string Name { get; set; } = "Baran";
}

public class NimbleMediatorNotificationWithSingleHandlerHandler : INotificationHandler<NimbleMediatorNotificationWithSingleHandler>
{
    public Task HandleAsync(NimbleMediatorNotificationWithSingleHandler notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}