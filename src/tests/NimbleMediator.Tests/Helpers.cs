using Microsoft.Extensions.DependencyInjection;
using NimbleMediator.ServiceExtensions;

namespace NimbleMediator.Tests;

public static class Helpers
{
    public static ServiceProvider SetupServiceProvider()
    {
        var services = new ServiceCollection();

        services.AddNimbleMediator(config =>
        {
            config.RegisterServicesFromAssembly(typeof(MyRequestWithResponse).Assembly);
        });

        return services.BuildServiceProvider();
    }
}
