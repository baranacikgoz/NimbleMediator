using MediatR;

namespace MediatorsBenchmark;

public class CreateUserRequestMediatR : IRequest<string>
{
    public string Name { get; set; } = "Baran";
}

public class CreateUserRequestMediatRHandler : IRequestHandler<CreateUserRequestMediatR, string>
{
    public Task<string> Handle(CreateUserRequestMediatR request, CancellationToken cancellationToken)
    {
        return Task.FromResult(request.Name);
    }
}