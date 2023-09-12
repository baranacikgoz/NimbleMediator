using NimbleMediator.Contracts;

namespace MediatorsBenchmark;

public class UserRegisteredNimbleNotification : INotification
{
    public string Name { get; set; } = "Baran";
}

public class UserRegisteredNimbleNotificationHandler : INotificationHandler<UserRegisteredNimbleNotification>
{
    public async Task HandleAsync(UserRegisteredNimbleNotification notification, CancellationToken cancellationToken)
    {

    }
}

public class DashboardStatsNimbleNotificationHandler : INotificationHandler<UserRegisteredNimbleNotification>
{
    public async Task HandleAsync(UserRegisteredNimbleNotification notification, CancellationToken cancellationToken)
    {

    }
}

public class NotifAdminsNimbleNotificationHandler : INotificationHandler<UserRegisteredNimbleNotification>
{
    public async Task HandleAsync(UserRegisteredNimbleNotification notification, CancellationToken cancellationToken)
    {

    }
}