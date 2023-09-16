﻿using BenchmarkDotNet.Attributes;
using NimbleMediator;
using Microsoft.Extensions.DependencyInjection;
using NimbleMediator.ServiceExtensions;
using NimbleMediator.Implementations;
using MediatR.NotificationPublishers;

namespace MediatorsBenchmark;

[MemoryDiagnoser]
public class SendBenchmark
{
    private MediatR.IMediator _mediatR;
    private NimbleMediator.Contracts.IMediator _nimbleMediator;

    [GlobalSetup]
    public void GlobalSetup()
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
    public async Task<string> MediatR_Send()
    {
        return await _mediatR.Send(new MediatRRequest(), CancellationToken.None);
    }

    [Benchmark]
    public async ValueTask<string> NimbleMediator_Send()
    {
        return await _nimbleMediator.SendAsync<NimbleMediatorRequest, string>(new NimbleMediatorRequest(), CancellationToken.None);
    }

    [Benchmark]
    public async Task<string> MediatR_Send_ThrowsException()
    {
        try
        {
            return await _mediatR.Send(new MediatRRequestThrowsException(), CancellationToken.None);
        }
        catch (Exception)
        {
            return "Benchmark";
        }
    }

    [Benchmark]
    public async ValueTask<string> NimbleMediator_Send_ThrowsException()
    {
        try
        {
            return await _nimbleMediator.SendAsync<NimbleMediatorRequestThrowsException, string>(new NimbleMediatorRequestThrowsException(), CancellationToken.None);
        }
        catch (Exception)
        {
            return "Benchmark";
        }
    }
}