## [1.0.0] & [1.0.1]

- Initial release.

## [1.0.2]

- Registered ISender and IPublisher also to the DI. This was forgotten in the initial release.

## [1.1.0]

- Removed the type dictionary for SendAsync, now directly depends on the DI container to retrieve the handler.
- Improved performance, now up to 3.5x faster and utilizes up to 16x less memory in certain cases.
- Fixed the method call order dependency in NimbleMediatorConfig; it is now order-independent.
- Overhauled notification handling; now supports notification publisher implementations via INotificationPublisher. Now maintains a Dictionary<Type, Type> for notifications.
- Added 3 default notification publishers: ForeachAwaitRobustPublisher, ForeachAwaitStopOnFirstExceptionPublisher, and TaskWhenAllPublisher.
- Made minor changes to method names and signatures in NimbleMediatorConfig.