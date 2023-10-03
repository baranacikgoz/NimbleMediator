using System.Runtime.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NimbleMediator.Contracts;
using NimbleMediator.NotificationPublishers;
using NimbleMediator.ServiceExtensions;
namespace NimbleMediator.Tests;

public class ServiceCollectionExtensionsTests
{

    public ServiceCollectionExtensionsTests()
    {
        var services = new ServiceCollection();

        services.AddNimbleMediator(config => config.RegisterServicesFromAssembly(typeof(MyRequestWithResponse).Assembly));

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
    public void ISender_Should_Be_Scoped_By_Default()
    {

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddNimbleMediator(config =>
        {
            config.RegisterServicesFromAssembly(typeof(MyRequestWithResponse).Assembly);
        });

        var sender = serviceCollection.FirstOrDefault(x => x.ServiceType == typeof(ISender));

        Assert.NotNull(sender);
        Assert.True(sender.Lifetime == ServiceLifetime.Scoped);
    }

    [Fact]
    public void DI_Should_Resolve_IPublisher()
    {
        var publisher = _serviceProvider.GetService<IPublisher>();

        Assert.NotNull(publisher);
        Assert.True(publisher is IPublisher p);
    }

    [Fact]
    public void IPublisher_Should_Be_Scoped_By_Default()
    {

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddNimbleMediator(config =>
        {
            config.RegisterServicesFromAssembly(typeof(MyRequestWithResponse).Assembly);
        });

        var publisher = serviceCollection.FirstOrDefault(x => x.ServiceType == typeof(IPublisher));

        Assert.NotNull(publisher);
        Assert.True(publisher.Lifetime == ServiceLifetime.Scoped);
    }

    [Fact]
    public void DI_Should_Resolve_IMediator()
    {
        var mediator = _serviceProvider.GetService<IMediator>();

        Assert.NotNull(mediator);
        Assert.True(mediator is IMediator m);
    }

    [Fact]
    public void IMediator_Should_Be_Scoped_By_Default()
    {

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddNimbleMediator(config =>
        {
            config.RegisterServicesFromAssembly(typeof(MyRequestWithResponse).Assembly);
        });

        var mediator = serviceCollection.FirstOrDefault(x => x.ServiceType == typeof(IMediator));

        Assert.NotNull(mediator);
        Assert.True(mediator.Lifetime == ServiceLifetime.Scoped);
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

    [Fact]
    public void Mediator_Should_Have_Singleton_If_Set_Explicitly()
    {
        var services = new ServiceCollection();

        services.AddNimbleMediator(config =>
        {
            config.SetMediatorLifetime(ServiceLifetime.Singleton);
            config.RegisterServicesFromAssembly(typeof(MyRequestWithResponse).Assembly);
        });

        var mediator = services.FirstOrDefault(x => x.ServiceType == typeof(IMediator));

        Assert.NotNull(mediator);
        Assert.True(mediator.Lifetime == ServiceLifetime.Singleton);
    }

    [Fact]
    public void Mediator_Should_Have_Scoped_If_Set_Explicitly()
    {
        var services = new ServiceCollection();

        services.AddNimbleMediator(config =>
        {
            config.SetMediatorLifetime(ServiceLifetime.Scoped);
            config.RegisterServicesFromAssembly(typeof(MyRequestWithResponse).Assembly);
        });

        var mediator = services.FirstOrDefault(x => x.ServiceType == typeof(IMediator));

        Assert.NotNull(mediator);
        Assert.True(mediator.Lifetime == ServiceLifetime.Scoped);
    }

    [Fact]
    public void Mediator_Should_Have_Transient_If_Set_Explicitly()
    {
        var services = new ServiceCollection();

        services.AddNimbleMediator(config =>
        {
            config.SetMediatorLifetime(ServiceLifetime.Transient);
            config.RegisterServicesFromAssembly(typeof(MyRequestWithResponse).Assembly);
        });

        var mediator = services.FirstOrDefault(x => x.ServiceType == typeof(IMediator));

        Assert.NotNull(mediator);
        Assert.True(mediator.Lifetime == ServiceLifetime.Transient);
    }
}