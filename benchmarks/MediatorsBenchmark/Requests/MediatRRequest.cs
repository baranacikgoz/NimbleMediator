using MediatR;

namespace MediatorsBenchmark;

public class MediatRRequest : IRequest<string>
{
    public string Name { get; set; } = "Baran";
}

public class MediatRRequestHandler : IRequestHandler<MediatRRequest, string>
{
    public Task<string> Handle(MediatRRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(request.Name);
    }
}