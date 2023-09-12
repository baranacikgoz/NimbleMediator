using MediatR;

namespace MediatorsBenchmark;

public class UserRegisteredMediatRNotification : INotification
{
    public string Name { get; set; } = "Baran";
}

public class UserRegisteredMediatRNotificationHandler : INotificationHandler<UserRegisteredMediatRNotification>
{
    public async Task Handle(UserRegisteredMediatRNotification notification, CancellationToken cancellationToken)
    {

    }
}

public class DashboardStatsMediatRNotificationHandler : INotificationHandler<UserRegisteredMediatRNotification>
{
    public async Task Handle(UserRegisteredMediatRNotification notification, CancellationToken cancellationToken)
    {

    }
}

public class NotifAdminsMediatRNotificationHandler : INotificationHandler<UserRegisteredMediatRNotification>
{
    public async Task Handle(UserRegisteredMediatRNotification notification, CancellationToken cancellationToken)
    {

    }
}