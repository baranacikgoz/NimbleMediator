using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NimbleMediator.Contracts;
using NimbleMediator.Implementations;
using NimbleMediator.NotificationPublishers;

namespace NimbleMediator;

/// <summary>
/// Configuration for <see cref="IMediator"/>.
/// </summary>
public class NimbleMediatorConfig
{
    private readonly IServiceCollection _services;
    private readonly Dictionary<Type, Type> _publisherTypeMappings;
    private Type _defaultPublisherType = typeof(ForeachAwaitRobustPublisher);
    private ServiceLifetime _defaultPublisherLifetime = ServiceLifetime.Singleton;

    public NimbleMediatorConfig(IServiceCollection services, Dictionary<Type, Type> publisherTypeMappings)
    {
        _services = services;
        _publisherTypeMappings = publisherTypeMappings;
    }

    /// <summary>
    /// Registers all requests, notifications and respective handlers from the given assembly.
    /// Notifications are registered with <see cref="NotificationPublisherType.Foreach"/> by default.
    /// You can change the publisher type for a notification type by calling <see cref="SetNotificationPublisherType{TNotification}(NotificationPublisherType)"/>.
    /// </summary>
    /// <param name="assembly"></param>
    public void RegisterServicesFromAssembly(Assembly assembly)
    {
        RegisterRequestsFromAssembly(assembly);
        RegisterNotificationsFromAssembly(assembly);

        // Register default publisher type
        TryAdd(_services, _defaultPublisherType, _defaultPublisherLifetime);
    }

    /// <summary>
    /// Sets the default publisher type for notifications.
    /// </summary>
    /// <param name="lifetime"></param>
    public void SetDefaultNotificationPublisherLifetime(ServiceLifetime lifetime)
    {
        _defaultPublisherLifetime = lifetime;

        TryAdd(_services, _defaultPublisherType, _defaultPublisherLifetime);
    }

    /// <summary>
    /// Sets the default publisher type for notifications.
    /// </summary>
    /// <param name="publisherType"></param>
    public void SetDefaultNotificationPublisherType<TNotificationPublisher>()
        where TNotificationPublisher : INotificationPublisher
    {
        var publisherType = typeof(TNotificationPublisher);
        _defaultPublisherType = publisherType;

        // If the publisher is not registered yet, we have to register it as well. 
        TryAdd(_services, publisherType, _defaultPublisherLifetime);
    }

    /// <summary>
    /// Sets the publisher type for the given notification type.
    /// </summary>
    /// <typeparam name="TNotification"></typeparam>
    /// <param name="publisherType"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public void SetNotificationPublisherType<TNotification, TNotificationPublisher>(ServiceLifetime? lifetime = null)
        where TNotification : INotification
        where TNotificationPublisher : INotificationPublisher
    {
        var notificationType = typeof(TNotification);
        var publisherType = typeof(TNotificationPublisher);

        _publisherTypeMappings[notificationType] = publisherType;

        TryAdd(_services, publisherType, lifetime ?? _defaultPublisherLifetime);
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

                    if (!_publisherTypeMappings.ContainsKey(notificationType))
                    {
                        _publisherTypeMappings.Add(notificationType, _defaultPublisherType);
                    }
                    else
                    {
                        _publisherTypeMappings[notificationType] = _defaultPublisherType;
                    }
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