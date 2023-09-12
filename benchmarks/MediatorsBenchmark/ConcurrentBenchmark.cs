using BenchmarkDotNet.Attributes;
using NimbleMediator;
using Microsoft.Extensions.DependencyInjection;
using NimbleMediator.ServiceExtensions;
using NimbleMediator.Implementations;
using MediatR.NotificationPublishers;

namespace MediatorsBenchmark;

[MemoryDiagnoser]
public class ConcurrentBenchmark
{
    private MediatR.IMediator _mediatR;
    private NimbleMediator.Contracts.IMediator _nimbleMediator;

    [GlobalSetup]
    public void ConcurrentSetup()
    {
        var services = new ServiceCollection();

        services.AddNimbleMediator(config =>
        {
            config.RegisterHandlersFromAssembly(typeof(CreateUserRequestNimbleMediator).Assembly);
            // config.SetNotificationPublisherType<UserRegisteredNimbleNotification>(NotificationPublisherType.TaskWhenAll);
            config.SetDefaultNotificationPublisherType(NotificationPublisherType.TaskWhenAll);
        });

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(CreateUserRequestMediatR).Assembly);
            cfg.NotificationPublisher = new TaskWhenAllPublisher();
        });

        var provider = services.BuildServiceProvider();

        _mediatR = provider.GetRequiredService<MediatR.IMediator>();
        _nimbleMediator = provider.GetRequiredService<NimbleMediator.Contracts.IMediator>();
    }

    [Benchmark]
    public async Task PublishConcurrentWithMediatR()
    {
        await _mediatR.Publish(new UserRegisteredMediatRNotification(), CancellationToken.None);
    }

    [Benchmark]
    public async Task PublishConcurrentWithNimbleMediator()
    {
        await _nimbleMediator.PublishAsync(new UserRegisteredNimbleNotification(), CancellationToken.None);
    }
}