using NimbleMediator.Contracts;

namespace MediatorsBenchmark;

public class CreateUserRequestNimbleMediator : IRequest<string>
{
    public string Name { get; set; } = "Baran";
}

public class CreateUserRequestNimbleMediatorHandler : IRequestHandler<CreateUserRequestNimbleMediator, string>
{
    public ValueTask<string> HandleAsync(CreateUserRequestNimbleMediator request, CancellationToken cancellationToken)
    {
        return ValueTask.FromResult(request.Name);
    }
}