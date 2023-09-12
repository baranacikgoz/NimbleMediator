namespace NimbleMediator.Contracts;

/// <summary>
/// Defines a mediator with the ability to send requests and publish notifications.
/// </summary>
public interface IMediator : ISender, IPublisher
{

}
