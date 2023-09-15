using NimbleMediator.Contracts;

namespace NimbleMediator.Tests;

public class MyRequestWithResponse : IRequest<string>
{
    public string Name { get; set; } = "Baran";
}

public class MyRequestWithResponseHandler : IRequestHandler<MyRequestWithResponse, string>
{
    public ValueTask<string> HandleAsync(MyRequestWithResponse request, CancellationToken cancellationToken)
    {
        return ValueTask.FromResult(request.Name);
    }
}
