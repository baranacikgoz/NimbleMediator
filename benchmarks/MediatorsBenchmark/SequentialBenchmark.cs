using BenchmarkDotNet.Attributes;
using NimbleMediator;
using Microsoft.Extensions.DependencyInjection;
using NimbleMediator.ServiceExtensions;
using NimbleMediator.Implementations;
using MediatR.NotificationPublishers;

namespace MediatorsBenchmark;

[MemoryDiagnoser]
public class SequentialBenchmark
{
    private MediatR.IMediator _mediatR;
    private NimbleMediator.Contracts.IMediator _nimbleMediator;

    [GlobalSetup]
    public void SequentialSetup()
    {
        var services = new ServiceCollection();

        services.AddNimbleMediator(config =>
        {
            config.RegisterHandlersFromAssembly(typeof(CreateUserRequestNimbleMediator).Assembly);
        });

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(CreateUserRequestMediatR).Assembly);
        });

        var provider = services.BuildServiceProvider();

        _mediatR = provider.GetRequiredService<MediatR.IMediator>();
        _nimbleMediator = provider.GetRequiredService<NimbleMediator.Contracts.IMediator>();
    }

    [Benchmark]
    public async Task<string> SendWithMediatR()
    {
        return await _mediatR.Send(new CreateUserRequestMediatR(), CancellationToken.None);
    }

    [Benchmark]
    public async ValueTask<string> SendWithNimbleMediator()
    {
        return await _nimbleMediator.SendAsync<CreateUserRequestNimbleMediator, string>(new CreateUserRequestNimbleMediator(), CancellationToken.None);
    }

    [Benchmark]
    public async Task PublishSequentiallyWithMediatR()
    {
        await _mediatR.Publish(new UserRegisteredMediatRNotification(), CancellationToken.None);
    }

    [Benchmark]
    public async Task PublishSequentiallyWithNimbleMediator()
    {
        await _nimbleMediator.PublishAsync(new UserRegisteredNimbleNotification(), CancellationToken.None);
    }
}