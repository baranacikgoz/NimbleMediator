using NimbleMediator.Contracts;

namespace MediatorsBenchmark;

public class NimbleMediatorRequestThrowsException : IRequest<string>
{
    public string Name { get; set; } = "Baran";
}

public class NimbleMediatorRequestThrowsExceptionHandler : IRequestHandler<NimbleMediatorRequestThrowsException, string>
{
    public ValueTask<string> HandleAsync(NimbleMediatorRequestThrowsException request, CancellationToken cancellationToken)
    {
        throw new Exception("Benchmark");
    }
}