using NimbleMediator.Contracts;

namespace MediatorsBenchmark;

public class NimbleMediatorRequest : IRequest<string>
{
    public string Name { get; set; } = "Baran";
}

public class NimbleMediatorRequestHandler : IRequestHandler<NimbleMediatorRequest, string>
{
    public ValueTask<string> HandleAsync(NimbleMediatorRequest request, CancellationToken cancellationToken)
    {
        return ValueTask.FromResult(request.Name);
    }
}