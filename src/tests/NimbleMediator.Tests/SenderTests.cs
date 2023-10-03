using Microsoft.Extensions.DependencyInjection;
using NimbleMediator.Contracts;

namespace NimbleMediator.Tests;

public class SenderTests
{
    public SenderTests()
    {
        _sender = Helpers.SetupServiceProvider().GetRequiredService<ISender>();
    }
    private readonly ISender _sender;

    [Fact]
    public async Task SendAsync_Should_Handle_Request()
    {
        var request = new MyRequestWithoutResponse();

        await _sender.SendAsync(request, CancellationToken.None);
    }

    [Fact]
    public async Task SendAsync_Should_Handle_Request_WithResponse()
    {

        var request = new MyRequestWithResponse()
        {
            Name = "Baran"
        };

        var result = await _sender.SendAsync<MyRequestWithResponse, string>(request, CancellationToken.None);

        Assert.True(result is string str && str.Equals("Baran"));
    }
}
