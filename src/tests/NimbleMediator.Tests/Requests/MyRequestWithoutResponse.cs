using NimbleMediator.Contracts;

namespace NimbleMediator.Tests;

public class MyRequestWithoutResponse : IRequest
{
    public string Name { get; set; } = "Baran";
}

public class MyRequestWithoutResponseHandler : IRequestHandler<MyRequestWithoutResponse>
{
    public ValueTask HandleAsync(MyRequestWithoutResponse request, CancellationToken cancellationToken)
    {
        return ValueTask.CompletedTask;
    }
}
