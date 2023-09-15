using Microsoft.Extensions.DependencyInjection;
using NimbleMediator.Contracts;
using NimbleMediator.ServiceExtensions;
namespace NimbleMediator.Tests;



public class ServiceCollectionExtensionsTests
{

    public ServiceCollectionExtensionsTests()
    {
        _serviceProvider = Helpers.SetupServiceProvider();
    }

    private readonly IServiceProvider _serviceProvider;

    [Fact]
    public void DI_Should_Resolve_ISender()
    {
        var sender = _serviceProvider.GetService<ISender>();

        Assert.NotNull(sender);
    }

    [Fact]
    public void DI_Should_Resolve_IPublisher()
    {
        var publisher = _serviceProvider.GetService<IPublisher>();

        Assert.NotNull(publisher);
    }

    [Fact]
    public void DI_Should_Resolve_IMediator()
    {
        var mediator = _serviceProvider.GetService<IMediator>();

        Assert.NotNull(mediator);
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

}