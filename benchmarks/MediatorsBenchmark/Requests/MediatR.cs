using MediatR;

namespace MediatorsBenchmark;

public record MediatRRequestWithoutResponse(string Name) : IRequest;

public class MediatRRequestWithoutResponseHandler : IRequestHandler<MediatRRequestWithoutResponse>
{
    public Task Handle(MediatRRequestWithoutResponse request, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}

public record MediatRRequestWithResponse(string Name) : IRequest<string>;

public class MediatRRequestWithResponseHandler : IRequestHandler<MediatRRequestWithResponse, string>
{
    public Task<string> Handle(MediatRRequestWithResponse request, CancellationToken cancellationToken)
    {
        return Task.FromResult(request.Name);
    }
}

public record MediatRRequestWithoutResponseThrowsException(string Name) : IRequest;

public class MediatRRequestWithoutResponseThrowsExceptionHandler : IRequestHandler<MediatRRequestWithoutResponseThrowsException>
{
    public Task Handle(MediatRRequestWithoutResponseThrowsException request, CancellationToken cancellationToken)
    {
        throw new Exception("Benchmark");
    }
}

public record MediatRRequestWithResponseThrowsException(string Name) : IRequest<string>;

public class MediatRRequestWithResponseThrowsExceptionHandler : IRequestHandler<MediatRRequestWithResponseThrowsException, string>
{
    public Task<string> Handle(MediatRRequestWithResponseThrowsException request, CancellationToken cancellationToken)
    {
        throw new Exception("Benchmark");
    }
}