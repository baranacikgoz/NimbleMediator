﻿using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using NimbleMediator.Contracts;
using NimbleMediator.Implementations;

namespace NimbleMediator.ServiceExtensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddNimbleMediator(this IServiceCollection services, Action<NimbleMediatorConfig> configAction)
    {
        var requestsAndHandlerTypes = new Dictionary<Type, Type>();
        services.AddSingleton(requestsAndHandlerTypes);

        var publisherTypeMappings = new Dictionary<Type, NotificationPublisherType>();
        services.AddSingleton(publisherTypeMappings);

        services.AddSingleton(sp =>
            new Mediator(
                    sp,
                    sp.GetRequiredService<Dictionary<Type, NotificationPublisherType>>()));

        services.AddSingleton<ISender>(sp => sp.GetRequiredService<Mediator>());
        services.AddSingleton<IPublisher>(sp => sp.GetRequiredService<Mediator>());
        services.AddSingleton<IMediator>(sp => sp.GetRequiredService<Mediator>());

        var config = new NimbleMediatorConfig(services);

        configAction(config);

        return services;
    }
}