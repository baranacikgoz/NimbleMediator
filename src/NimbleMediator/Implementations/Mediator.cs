
using Microsoft.Extensions.DependencyInjection;
using NimbleMediator.Contracts;

namespace NimbleMediator.Implementations;

/// <summary>
/// Implementation of <see cref="IMediator"/>.
/// </summary>
public partial class Mediator : IMediator
{
    private readonly IServiceProvider _serviceProvider;
    private readonly Dictionary<Type, Type> _publisherTypeMappings;

    public Mediator(IServiceProvider serviceProvider, Dictionary<Type, Type> publisherTypeMappings)
    {
        _serviceProvider = serviceProvider;
        _publisherTypeMappings = publisherTypeMappings;
    }
}
