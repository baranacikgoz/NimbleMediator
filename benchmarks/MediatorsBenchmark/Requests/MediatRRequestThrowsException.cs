using MediatR;

namespace MediatorsBenchmark;

public class MediatRRequestThrowsException : IRequest<string>
{
    public string Name { get; set; } = "Baran";
}

public class MediatRRequestThrowsExceptionHandler : IRequestHandler<MediatRRequestThrowsException, string>
{
    public Task<string> Handle(MediatRRequestThrowsException request, CancellationToken cancellationToken)
    {
        throw new Exception("Benchmark");
    }
}