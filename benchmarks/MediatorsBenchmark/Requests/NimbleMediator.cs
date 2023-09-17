using NimbleMediator.Contracts;

namespace MediatorsBenchmark;

public record NimbleMediatorRequestWithoutResponse(string Name) : IRequest;

public class NimbleMediatorRequestWithoutResponseHandler : IRequestHandler<NimbleMediatorRequestWithoutResponse>
{
    public ValueTask HandleAsync(NimbleMediatorRequestWithoutResponse request, CancellationToken cancellationToken)
    {
        return ValueTask.CompletedTask;
    }
}

public record NimbleMediatorRequestWithResponse(string Name) : IRequest<string>;

public class NimbleMediatorRequestWithResponseHandler : IRequestHandler<NimbleMediatorRequestWithResponse, string>
{
    public ValueTask<string> HandleAsync(NimbleMediatorRequestWithResponse request, CancellationToken cancellationToken)
    {
        return ValueTask.FromResult(request.Name);
    }
}

public record NimbleMediatorRequestWithoutResponseThrowsException(string Name) : IRequest;

public class NimbleMediatorRequestWithoutResponseThrowsExceptionHandler : IRequestHandler<NimbleMediatorRequestWithoutResponseThrowsException>
{
    public ValueTask HandleAsync(NimbleMediatorRequestWithoutResponseThrowsException request, CancellationToken cancellationToken)
    {
        throw new Exception("Benchmark");
    }
}

public record NimbleMediatorRequestWithResponseThrowsException(string Name) : IRequest<string>;

public class NimbleMediatorRequestWithResponseThrowsExceptionHandler : IRequestHandler<NimbleMediatorRequestWithResponseThrowsException, string>
{
    public ValueTask<string> HandleAsync(NimbleMediatorRequestWithResponseThrowsException request, CancellationToken cancellationToken)
    {
        throw new Exception("Benchmark");
    }
}