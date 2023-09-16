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
    public ForeachAwaitBenchmark()
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
    private readonly MediatR.IMediator _mediatR;
    private readonly NimbleMediator.Contracts.IMediator _nimbleMediator;


    [Benchmark]
    public async Task MediatR_Publish_ForeachAwait_notification_has_1_handler()
    {
        await _mediatR.Publish(new MediatRNotificationWithSingleHandler(), CancellationToken.None);
    }

    [Benchmark]
    public async Task NimbleMediator_Publish_ForeachAwait_notification_has_1_handler()
    {
        await _nimbleMediator.PublishAsync(new NimbleMediatorNotificationWithSingleHandler(), CancellationToken.None);
    }

    [Benchmark]
    public async Task MediatR_Publish_ForeachAwait_notification_has_3_handlers()
    {
        await _mediatR.Publish(new MediatRNotificationWith3Handlers(), CancellationToken.None);
    }

    [Benchmark]
    public async Task NimbleMediator_Publish_ForeachAwait_notification_has_3_handlers()
    {
        await _nimbleMediator.PublishAsync(new NimbleMediatorNotificationWith3Handlers(), CancellationToken.None);
    }

    [Benchmark]
    public async Task MediatR_Publish_ForeachAwait_notification_has_3_handlers_1_throws_exception()
    {
        try
        {
            await _mediatR.Publish(new MediatRNotificationWith3Handlers1ThrowsException(), CancellationToken.None);
        }
        catch (Exception)
        {
        }
    }

    [Benchmark]
    public async Task NimbleMediator_Publish_ForeachAwait_notification_has_3_handlers_1_throws_exception()
    {
        try
        {
            await _nimbleMediator.PublishAsync(new NimbleMediatorNotificationWith3Handlers1ThrowsException(), CancellationToken.None);
        }
        catch (Exception)
        {
        }
    }
}