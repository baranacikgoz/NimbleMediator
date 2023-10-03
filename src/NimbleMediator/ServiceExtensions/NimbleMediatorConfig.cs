using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NimbleMediator.Contracts;
using NimbleMediator.Implementations;
using NimbleMediator.NotificationPublishers;

namespace NimbleMediator.ServiceExtensions;

/// <summary>
/// Configuration for <see cref="IMediator"/>.
/// </summary>
public class NimbleMediatorConfig
{
    private readonly IServiceCollection _services;
    private readonly Dictionary<Type, Type> _publisherTypeMappings = new();
    private Type _defaultPublisherType = typeof(ForeachAwaitRobustPublisher);
    private ServiceLifetime _defaultPublisherLifetime = ServiceLifetime.Singleton;
    private ServiceLifetime _mediatorLifetime = ServiceLifetime.Scoped;
    private readonly HashSet<Assembly> _assemblies = new();

    public NimbleMediatorConfig(IServiceCollection services) => _services = services;

    /// <summary>
    /// Registers all requests, notifications and respective handlers from the given assembly.
    /// </summary>
    /// <param name="assembly"></param>
    public void RegisterServicesFromAssembly(Assembly assembly) => _assemblies.Add(assembly);

    /// <summary>
    /// Registers all requests, notifications and respective handlers from the given assemblies.
    /// </summary>
    /// <param name="assemblies"></param>
    public void RegisterServicesFromAssemblies(params Assembly[] assemblies)
    {
        foreach (var assembly in assemblies)
        {
            RegisterServicesFromAssembly(assembly);
        }
    }

    /// <summary>
    /// Sets the lifetime of the mediator implementation and IMediator, ISender, IPublisher interfaces.
    /// </summary>
    /// <param name="lifetime"></param>
    public void SetMediatorLifetime(ServiceLifetime lifetime) => _mediatorLifetime = lifetime;

    /// <summary>
    /// Sets the default publisher type for notifications.
    /// </summary>
    /// <param name="lifetime"></param>
    public void SetDefaultNotificationPublisherLifetime(ServiceLifetime lifetime) => _defaultPublisherLifetime = lifetime;

    /// <summary>
    /// Sets the default publisher type for notifications.
    /// </summary>
    /// <param name="publisherType"></param>
    public void SetDefaultNotificationPublisher<TNotificationPublisher>(ServiceLifetime? lifetime = null)
        where TNotificationPublisher : INotificationPublisher
    {
        var publisherType = typeof(TNotificationPublisher);
        _defaultPublisherType = publisherType;

        if (lifetime is not null)
        {
            SetDefaultNotificationPublisherLifetime(lifetime.Value);
        }
    }

    /// <summary>
    /// Sets the publisher type for the given notification type.
    /// </summary>
    /// <typeparam name="TNotification"></typeparam>
    /// <param name="publisherType"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public void SetNotificationPublisher<TNotification, TNotificationPublisher>(ServiceLifetime? lifetime = null)
        where TNotification : INotification
        where TNotificationPublisher : INotificationPublisher
    {
        var notificationType = typeof(TNotification);
        var publisherType = typeof(TNotificationPublisher);

        _publisherTypeMappings[notificationType] = publisherType;

        TryAdd(_services, publisherType, lifetime ?? _defaultPublisherLifetime);
    }

    /// <summary>
    /// The public Register methods are actually just adds assemblies to a hashset, not registering them immediately.
    /// This is needed for the config to not to be dependent on the order of the calls.
    /// This method is called internally to register all marked assemblies.
    /// </summary>
    internal void RegisterServicesInternal()
    {
        RegisterMediatorImplementationAndInterfaces(_services, _mediatorLifetime, _publisherTypeMappings);

        foreach (var assembly in _assemblies)
        {
            RegisterRequestsFromAssembly(assembly);
            RegisterNotificationsFromAssembly(assembly);
        }

        // Register default publisher type if not provided by user.
        TryAdd(_services, _defaultPublisherType, _defaultPublisherLifetime);
    }

    private static void RegisterMediatorImplementationAndInterfaces(IServiceCollection services, ServiceLifetime mediatorLifetime, Dictionary<Type, Type> publisherTypeMappings)
    {
        services.Add(
            new ServiceDescriptor(typeof(Mediator),
            sp => new Mediator(sp, publisherTypeMappings),
            mediatorLifetime));

        services.Add(
            new ServiceDescriptor(typeof(IMediator),
            sp => sp.GetRequiredService<Mediator>(),
            mediatorLifetime));

        services.Add(
            new ServiceDescriptor(typeof(ISender),
            sp => sp.GetRequiredService<Mediator>(),
            mediatorLifetime));

        services.Add(
            new ServiceDescriptor(typeof(IPublisher),
            sp => sp.GetRequiredService<Mediator>(),
            mediatorLifetime));
    }

    private void RegisterRequestsFromAssembly(Assembly assembly)
    {
        foreach (var type in assembly.GetTypes())
        {
            foreach (var @interface in type.GetInterfaces())
            {
                if (@interface.IsGenericType
                    && (@interface.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)
                        || @interface.GetGenericTypeDefinition() == typeof(IRequestHandler<>)))
                {
                    _services.AddTransient(@interface, type);
                }
            }
        }
    }

    private void RegisterNotificationsFromAssembly(Assembly assembly)
    {
        foreach (var type in assembly.GetTypes())
        {
            foreach (var @interface in type.GetInterfaces())
            {
                if (@interface.IsGenericType && @interface.GetGenericTypeDefinition() == typeof(INotificationHandler<>))
                {
                    var notificationType = @interface.GetGenericArguments()[0];
                    var handlerType = type;

                    _services.AddTransient(@interface, handlerType);

                    _publisherTypeMappings.TryAdd(notificationType, _defaultPublisherType);

                }
            }
        }
    }

    private static void TryAdd(IServiceCollection services, Type type, ServiceLifetime lifetime)
    {
        switch (lifetime)
        {
            case ServiceLifetime.Singleton:
                services.TryAddSingleton(type);
                break;

            case ServiceLifetime.Scoped:
                services.TryAddScoped(type);
                break;

            case ServiceLifetime.Transient:
                services.TryAddTransient(type);
                break;
        }
    }
}