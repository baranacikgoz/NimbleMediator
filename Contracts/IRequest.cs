namespace NimbleMediator.Contracts;
#pragma warning disable S2326 // Unused type parameters should be removed

/// <summary>
/// Marker interface for requests with response.
/// </summary>
/// <typeparam name="TResponse"></typeparam>

public interface IRequest<TResponse>
{
}

/// <summary>
/// Marker interface for requests without response.
/// </summary>
public interface IRequest
{
}

#pragma warning restore S2326