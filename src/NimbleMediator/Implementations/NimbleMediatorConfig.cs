using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using NimbleMediator.Contracts;
using NimbleMediator.Implementations;

namespace NimbleMediator;

/// <summary>
/// Configuration for <see cref="IMediator"/>.
/// </summary>
public class NimbleMediatorConfig
{
    private readonly IServiceCollection _services;
    private readonly Dictionary<Type, Type> _requestsAndHandlerTypes;
    private readonly Dictionary<Type, NotificationPublisherType> _publisherTypeMappings;
    private NotificationPublisherType _defaultPublisherType = NotificationPublisherType.Foreach;

    public NimbleMediatorConfig(IServiceCollection services)
    {
        _services = services;
        var serviceCollection = _services.BuildServiceProvider();

        _requestsAndHandlerTypes = serviceCollection.GetRequiredService<Dictionary<Type, Type>>();
        _publisherTypeMappings = serviceCollection.GetRequiredService<Dictionary<Type, NotificationPublisherType>>();
    }

    /// <summary>
    /// Registers all requests, notifications and respective handlers from the given assembly.
    /// Notifications are registered with <see cref="NotificationPublisherType.Foreach"/> by default.
    /// You can change the publisher type for a notification type by calling <see cref="SetNotificationPublisherType{TNotification}(NotificationPublisherType)"/>.
    /// </summary>
    /// <param name="assembly"></param>
    public void RegisterHandlersFromAssembly(Assembly assembly)
    {
        RegisterRequestsFromAssembly(assembly);
        RegisterNotificationsFromAssembly(assembly);
    }

    /// <summary>
    /// Sets the default publisher type for notifications.
    /// </summary>
    /// <param name="publisherType"></param>
    public void SetDefaultNotificationPublisherType(NotificationPublisherType publisherType)
        => _defaultPublisherType = publisherType;

    /// <summary>
    /// Sets the publisher type for the given notification type.
    /// </summary>
    /// <typeparam name="TNotification"></typeparam>
    /// <param name="publisherType"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public void SetNotificationPublisherType<TNotification>(NotificationPublisherType publisherType)
        where TNotification : INotification
    {
        var notificationType = typeof(TNotification);

        if (!_publisherTypeMappings.ContainsKey(notificationType))
        {
            throw new InvalidOperationException($"Notification {notificationType.Name} not found");
        }

        _publisherTypeMappings[notificationType] = publisherType;
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
                    var requestType = @interface.GetGenericArguments()[0];
                    var handlerType = type;
                    _services.AddTransient(handlerType);
                    _requestsAndHandlerTypes[requestType] = handlerType;
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
}