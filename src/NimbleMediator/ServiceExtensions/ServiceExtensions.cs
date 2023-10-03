using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using NimbleMediator.Contracts;
using NimbleMediator.Implementations;

namespace NimbleMediator.ServiceExtensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddNimbleMediator(this IServiceCollection services, Action<NimbleMediatorConfig> configAction)
    {
        var config = new NimbleMediatorConfig(services);

        configAction(config);

        config.RegisterServicesInternal();

        return services;
    }
}