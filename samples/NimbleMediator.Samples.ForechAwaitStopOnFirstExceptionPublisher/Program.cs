using Microsoft.Extensions.DependencyInjection;
using NimbleMediator.Contracts;
using NimbleMediator.NotificationPublishers;
using NimbleMediator.ServiceExtensions;

var services = new ServiceCollection();

services.AddNimbleMediator(config =>
{
    config.SetDefaultNotificationPublisherType<ForeachAwaitStopOnFirstExceptionPublisher>();
    config.RegisterServicesFromAssembly(typeof(MyNotification).Assembly);
});

var provider = services.BuildServiceProvider();

var mediator = provider.GetRequiredService<IMediator>();

try
{
    await mediator.PublishAsync(new MyNotification(), CancellationToken.None);
}
catch (AggregateException ae)
{
    // This will not happen because the publisher stops on first exception.
    Console.WriteLine("AggregateException:");
    foreach (var e in ae.InnerExceptions)
    {
        Console.WriteLine(e.Message);
    }
}
catch (Exception e)
{
    // This is the expected exception.

    Console.WriteLine("Exception:");
    Console.WriteLine(e.Message);
}

public class MyNotification : INotification
{
    public string Name { get; set; }
}

public class MyNotificationHandler1 : INotificationHandler<MyNotification>
{
    public Task HandleAsync(MyNotification notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Handling notification from {nameof(MyNotificationHandler1)}");
        return Task.CompletedTask;
    }
}

public class MyNotificationHandler2 : INotificationHandler<MyNotification>
{
    public Task HandleAsync(MyNotification notification, CancellationToken cancellationToken)
    {
        throw new Exception("Exception from MyNotificationHandler2");
    }
}

public class MyNotificationHandler3 : INotificationHandler<MyNotification>
{
    public Task HandleAsync(MyNotification notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Handling notification from {nameof(MyNotificationHandler3)}");
        return Task.CompletedTask;
    }
}