using BenchmarkDotNet.Attributes;
using NimbleMediator;
using Microsoft.Extensions.DependencyInjection;
using NimbleMediator.ServiceExtensions;
using NimbleMediator.Implementations;
using MediatR.NotificationPublishers;

namespace MediatorsBenchmark;

[MemoryDiagnoser]
public class ForeachAwaitBenchmark
{
    private MediatR.IMediator _mediatR;
    private NimbleMediator.Contracts.IMediator _nimbleMediator;

    [GlobalSetup]
    public void SequentialSetup()
    {
        var services = new ServiceCollection();

        services.AddNimbleMediator(config =>
        {
            config.RegisterHandlersFromAssembly(typeof(NimbleMediatorRequest).Assembly);
        });

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(MediatRRequest).Assembly);
        });

        var provider = services.BuildServiceProvider();

        _mediatR = provider.GetRequiredService<MediatR.IMediator>();
        _nimbleMediator = provider.GetRequiredService<NimbleMediator.Contracts.IMediator>();
    }

    [Benchmark]
    public async Task Publish_ForeachAwait_with_MediatR_the_notification_has_single_handler()
    {
        await _mediatR.Publish(new MediatRNotificationWithSingleHandler(), CancellationToken.None);
    }

    [Benchmark]
    public async Task Publish_ForeachAwait_with_Nimble_Mediator_the_notification_has_single_handler()
    {
        await _nimbleMediator.PublishAsync(new NimbleMediatorNotificationWithSingleHandler(), CancellationToken.None);
    }

    [Benchmark]
    public async Task Publish_ForeachAwait_with_MediatR_the_notification_has_multiple_handlers()
    {
        await _mediatR.Publish(new MediatRNotificationWithMultipleHandlers(), CancellationToken.None);
    }

    [Benchmark]
    public async Task Publish_ForeachAwait_with_Nimble_Mediator_the_notification_has_multiple_handlers()
    {
        await _nimbleMediator.PublishAsync(new NimbleMediatorNotificationWithMultipleHandlers(), CancellationToken.None);
    }
}