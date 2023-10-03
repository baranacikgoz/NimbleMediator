using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using MediatorsBenchmark;
using Microsoft.Extensions.DependencyInjection;
using NimbleMediator;
using NimbleMediator.NotificationPublishers;
using NimbleMediator.ServiceExtensions;

namespace MediatorsBenchmark;

[MemoryDiagnoser]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[HideColumns(new string[] { "Error", "StdDev", "RatioSD", "Gen0" })]
public class ForeachAwaitBenchmark
{
    public ForeachAwaitBenchmark()
    {
        var services = new ServiceCollection();

        services.AddNimbleMediator(config =>
        {
            config.SetDefaultNotificationPublisher<ForeachAwaitStopOnFirstExceptionPublisher>();
            config.RegisterServicesFromAssembly(typeof(NimbleMediatorRequestWithoutResponse).Assembly);
        });

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(MediatRRequestWithoutResponse).Assembly);
            cfg.NotificationPublisher = new MediatR.NotificationPublishers.ForeachAwaitPublisher();
        });

        var provider = services.BuildServiceProvider();

        _mediatR = provider.GetRequiredService<MediatR.IMediator>();
        _nimbleMediator = provider.GetRequiredService<NimbleMediator.Contracts.IMediator>();
    }

    private readonly MediatR.IMediator _mediatR;
    private readonly NimbleMediator.Contracts.IMediator _nimbleMediator;

    private readonly NimbleMediatorNotificationWith1Handler _nimbleMediatorNotificationWith1Handler = new("Test");
    private readonly NimbleMediatorNotificationWith1HandlerThrowsException _nimbleMediatorNotificationWith1HandlerThrowsException = new("Test");
    private readonly NimbleMediatorNotificationWith3Handlers _nimbleMediatorNotificationWith3Handlers = new("Test");
    private readonly NimbleMediatorNotificationWith3Handlers1ThrowsException _nimbleMediatorNotificationWith3Handlers1ThrowsException = new("Test");
    private readonly MediatRNotificationWith1Handler _mediatRNotificationWith1Handler = new("Test");
    private readonly MediatRNotificationWith1HandlerThrowsException _mediatRNotificationWith1HandlerThrowsException = new("Test");
    private readonly MediatRNotificationWith3Handlers _mediatRNotificationWith3Handlers = new("Test");
    private readonly MediatRNotificationWith3Handlers1ThrowsException _mediatRNotificationWith3Handlers1ThrowsException = new("Test");

    [BenchmarkCategory("1"), Benchmark(Baseline = true)]
    public async Task NimbleMediator_Publish_ForeachAwait_notification_has_1_handler()
    {
        await _nimbleMediator.PublishAsync(_nimbleMediatorNotificationWith1Handler, CancellationToken.None);
    }

    [BenchmarkCategory("1"), Benchmark]
    public async Task MediatR_Publish_ForeachAwait_notification_has_1_handler()
    {
        await _mediatR.Publish(_mediatRNotificationWith1Handler, CancellationToken.None);
    }

    [BenchmarkCategory("2"), Benchmark(Baseline = true)]
    public async Task NimbleMediator_Publish_ForeachAwait_notification_has_1_handler_ThrowsException()
    {
        try
        {
            await _nimbleMediator.PublishAsync(_nimbleMediatorNotificationWith1HandlerThrowsException, CancellationToken.None);
        }
        catch (Exception)
        {
            // ignored
        }
    }

    [BenchmarkCategory("2"), Benchmark]
    public async Task MediatR_Publish_ForeachAwait_notification_has_1_handler_ThrowsException()
    {
        try
        {
            await _mediatR.Publish(_mediatRNotificationWith1HandlerThrowsException, CancellationToken.None);
        }
        catch (Exception)
        {
            // ignored
        }
    }

    [BenchmarkCategory("3"), Benchmark(Baseline = true)]
    public async Task NimbleMediator_Publish_ForeachAwait_notification_has_3_handlers()
    {
        await _nimbleMediator.PublishAsync(_nimbleMediatorNotificationWith3Handlers, CancellationToken.None);
    }

    [BenchmarkCategory("3"), Benchmark]
    public async Task MediatR_Publish_ForeachAwait_notification_has_3_handlers()
    {
        await _mediatR.Publish(_mediatRNotificationWith3Handlers, CancellationToken.None);
    }

    [BenchmarkCategory("4"), Benchmark(Baseline = true)]
    public async Task NimbleMediator_Publish_ForeachAwait_notification_has_3_handlers_1_throws_exception()
    {
        try
        {
            await _nimbleMediator.PublishAsync(_nimbleMediatorNotificationWith3Handlers1ThrowsException, CancellationToken.None);
        }
        catch (Exception)
        {
            // ignored
        }
    }

    [BenchmarkCategory("4"), Benchmark]
    public async Task MediatR_Publish_ForeachAwait_notification_has_3_handlers_1_throws_exception()
    {
        try
        {
            await _mediatR.Publish(_mediatRNotificationWith3Handlers1ThrowsException, CancellationToken.None);
        }
        catch (Exception)
        {
            // ignored
        }
    }

}