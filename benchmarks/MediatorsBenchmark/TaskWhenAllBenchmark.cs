using BenchmarkDotNet.Attributes;
using NimbleMediator;
using Microsoft.Extensions.DependencyInjection;
using NimbleMediator.ServiceExtensions;
using NimbleMediator.Implementations;
using MediatR.NotificationPublishers;

namespace MediatorsBenchmark;

[MemoryDiagnoser]
public class TaskWhenAllBenchmark
{
    private MediatR.IMediator _mediatR;
    private NimbleMediator.Contracts.IMediator _nimbleMediator;

    [GlobalSetup]
    public void TaskWhenAllSetup()
    {
        var services = new ServiceCollection();

        services.AddNimbleMediator(config =>
        {
            config.RegisterHandlersFromAssembly(typeof(NimbleMediatorRequest).Assembly);
            // config.SetNotificationPublisherType<UserRegisteredNimbleNotification>(NotificationPublisherType.TaskWhenAll);
            config.SetDefaultNotificationPublisherType(NotificationPublisherType.TaskWhenAll);
        });

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(MediatRRequest).Assembly);
            cfg.NotificationPublisher = new TaskWhenAllPublisher();
        });

        var provider = services.BuildServiceProvider();

        _mediatR = provider.GetRequiredService<MediatR.IMediator>();
        _nimbleMediator = provider.GetRequiredService<NimbleMediator.Contracts.IMediator>();
    }

    [Benchmark]
    public async Task MediatR_Publish_TaskWhenAll_notification_has_1_handler()
    {
        await _mediatR.Publish(new MediatRNotificationWithSingleHandler(), CancellationToken.None);
    }

    [Benchmark]
    public async Task NimbleMediator_Publish_TaskWhenAll_notification_has_1_handler()
    {
        await _nimbleMediator.PublishAsync(new NimbleMediatorNotificationWithSingleHandler(), CancellationToken.None);
    }

    [Benchmark]
    public async Task MediatR_Publish_TaskWhenAll_notification_has_3_handlers()
    {
        await _mediatR.Publish(new MediatRNotificationWith3Handlers(), CancellationToken.None);
    }

    [Benchmark]
    public async Task NimbleMediator_Publish_TaskWhenAll_notification_has_3_handlers()
    {
        await _nimbleMediator.PublishAsync(new NimbleMediatorNotificationWith3Handlers(), CancellationToken.None);
    }

}