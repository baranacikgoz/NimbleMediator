using BenchmarkDotNet.Attributes;
using NimbleMediator;
using Microsoft.Extensions.DependencyInjection;
using NimbleMediator.ServiceExtensions;
using NimbleMediator.Implementations;
using MediatR.NotificationPublishers;

namespace MediatorsBenchmark;

[MemoryDiagnoser]
public class SendBenchmark
{
    public SendBenchmark()
    {
        var services = new ServiceCollection();

        services.AddNimbleMediator(config =>
        {
            config.RegisterServicesFromAssembly(typeof(NimbleMediatorRequestWithoutResponse).Assembly);
        });

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(MediatRRequestWithoutResponse).Assembly);
        });

        var provider = services.BuildServiceProvider();

        _mediatR = provider.GetRequiredService<MediatR.IMediator>();
        _nimbleMediator = provider.GetRequiredService<NimbleMediator.Contracts.IMediator>();
    }
    private readonly MediatR.IMediator _mediatR;
    private readonly NimbleMediator.Contracts.IMediator _nimbleMediator;

    private readonly NimbleMediatorRequestWithoutResponse _nimbleMediatorRequestWithoutResponse = new("Test");
    private readonly NimbleMediatorRequestWithResponse _nimbleMediatorRequestWithResponse = new("Test");
    private readonly MediatRRequestWithoutResponse _mediatRRequestWithoutResponse = new("Test");
    private readonly MediatRRequestWithResponse _mediatRRequestWithResponse = new("Test");
    private readonly NimbleMediatorRequestWithoutResponseThrowsException _nimbleMediatorRequestWithoutResponseThrowsException = new("Test");
    private readonly MediatRRequestWithoutResponseThrowsException _mediatRRequestWithoutResponseThrowsException = new("Test");
    private readonly NimbleMediatorRequestWithResponseThrowsException _nimbleMediatorRequestWithResponseThrowsException = new("Test");
    private readonly MediatRRequestWithResponseThrowsException _mediatRRequestWithResponseThrowsException = new("Test");


    [Benchmark]
    public async ValueTask NimbleMediator_Send_WithoutResponse()
    {
        await _nimbleMediator.SendAsync(_nimbleMediatorRequestWithoutResponse, CancellationToken.None);
    }

    [Benchmark]
    public async ValueTask MediatR_Send_WithoutResponse()
    {
        await _mediatR.Send(_mediatRRequestWithoutResponse, CancellationToken.None);
    }

    [Benchmark]
    public async ValueTask<string> NimbleMediator_Send_WithResponse()
    {
        return await _nimbleMediator.SendAsync<NimbleMediatorRequestWithResponse, string>(_nimbleMediatorRequestWithResponse, CancellationToken.None);
    }

    [Benchmark]
    public async Task<string> MediatR_Send_WithResponse()
    {
        return await _mediatR.Send(_mediatRRequestWithResponse, CancellationToken.None);
    }

    [Benchmark]
    public async Task NimbleMediator_Send_WithoutResponse_ThrowsException()
    {
        try
        {
            await _nimbleMediator.SendAsync(_nimbleMediatorRequestWithoutResponseThrowsException, CancellationToken.None);
        }
        catch
        {
            // ignored
        }
    }

    [Benchmark]
    public async Task MediatR_Send_WithoutResponse_ThrowsException()
    {
        try
        {
            await _mediatR.Send(_mediatRRequestWithoutResponseThrowsException, CancellationToken.None);
        }
        catch
        {
            // ignored
        }
    }

    [Benchmark]
    public async Task<string> NimbleMediator_Send_WithResponse_ThrowsException()
    {
        try
        {
            return await _nimbleMediator.SendAsync<NimbleMediatorRequestWithResponseThrowsException, string>(_nimbleMediatorRequestWithResponseThrowsException, CancellationToken.None);
        }
        catch
        {
            return "Test";
        }
    }

    [Benchmark]
    public async Task<string> MediatR_Send_WithResponse_ThrowsException()
    {
        try
        {
            return await _mediatR.Send(_mediatRRequestWithResponseThrowsException, CancellationToken.None);
        }
        catch
        {
            return "Test";
        }
    }
}