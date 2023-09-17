using System.Runtime.Serialization;
using Microsoft.Extensions.DependencyInjection;
using NimbleMediator.Contracts;
using NimbleMediator.NotificationPublishers;
using NimbleMediator.ServiceExtensions;
namespace NimbleMediator.Tests;



public class ServiceCollectionExtensionsTests
{

    public ServiceCollectionExtensionsTests()
    {
        var services = new ServiceCollection();

        services.AddNimbleMediator(config =>
        {
            config.RegisterServicesFromAssembly(typeof(MyRequestWithResponse).Assembly);
        });

        _serviceProvider = services.BuildServiceProvider();
    }

    private readonly IServiceProvider _serviceProvider;

    [Fact]
    public void DI_Should_Resolve_ISender()
    {
        var sender = _serviceProvider.GetService<ISender>();

        Assert.NotNull(sender);
        Assert.True(sender is ISender s);
    }

    [Fact]
    public void DI_Should_Resolve_IPublisher()
    {
        var publisher = _serviceProvider.GetService<IPublisher>();

        Assert.NotNull(publisher);
        Assert.True(publisher is IPublisher p);
    }

    [Fact]
    public void DI_Should_Resolve_IMediator()
    {
        var mediator = _serviceProvider.GetService<IMediator>();

        Assert.NotNull(mediator);
        Assert.True(mediator is IMediator m);
    }

    [Fact]
    public void DI_Should_Resolve_RequestHandler_TResponse()
    {
        var requestHandler = _serviceProvider.GetService<IRequestHandler<MyRequestWithResponse, string>>();

        Assert.NotNull(requestHandler);
    }

    [Fact]
    public void DI_Should_Resolve_RequestHandler()
    {
        var requestHandler = _serviceProvider.GetService<IRequestHandler<MyRequestWithoutResponse>>();

        Assert.NotNull(requestHandler);
    }

    [Fact]
    public void DI_Should_Resolve_ForeachAwaitRobustPublisher()
    {

        var services = new ServiceCollection();

        services.AddNimbleMediator(config =>
        {
            config.SetDefaultNotificationPublisher<ForeachAwaitRobustPublisher>();
            config.RegisterServicesFromAssembly(typeof(MyRequestWithResponse).Assembly);
        });

        var serviceProvider = services.BuildServiceProvider();
        var notificationHandler = serviceProvider.GetService<ForeachAwaitRobustPublisher>();

        Assert.NotNull(notificationHandler);
        Assert.True(notificationHandler is ForeachAwaitRobustPublisher publisher);
    }

    [Fact]
    public void DI_Should_Resolve_ForeachAwaitStopOnFirstExceptionPublisher()
    {
        var services = new ServiceCollection();

        services.AddNimbleMediator(config =>
        {
            config.SetDefaultNotificationPublisher<ForeachAwaitStopOnFirstExceptionPublisher>();
            config.RegisterServicesFromAssembly(typeof(MyRequestWithResponse).Assembly);
        });

        var serviceProvider = services.BuildServiceProvider();
        var notificationHandler = serviceProvider.GetService<ForeachAwaitStopOnFirstExceptionPublisher>();

        Assert.NotNull(notificationHandler);
        Assert.True(notificationHandler is ForeachAwaitStopOnFirstExceptionPublisher publisher);
    }

    [Fact]
    public void DI_Should_Resolve_TaskWhenAllPublisher()
    {
        var services = new ServiceCollection();

        services.AddNimbleMediator(config =>
        {
            config.SetDefaultNotificationPublisher<TaskWhenAllPublisher>();
            config.RegisterServicesFromAssembly(typeof(MyRequestWithResponse).Assembly);
        });

        var serviceProvider = services.BuildServiceProvider();
        var notificationHandler = serviceProvider.GetService<TaskWhenAllPublisher>();

        Assert.NotNull(notificationHandler);
        Assert.True(notificationHandler is TaskWhenAllPublisher publisher);
    }

}