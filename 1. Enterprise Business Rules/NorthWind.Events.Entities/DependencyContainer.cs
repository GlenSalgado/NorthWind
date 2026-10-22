namespace Microsoft.Extensions.DependencyInjection;

using NorthWind.Events.Entities.Interfaces;
using NorthWind.Events.Entities.Services;

public static class DependencyContainer
{
    public static IServiceCollection AddEventServices(
        this IServiceCollection services)
    {
        services.AddScoped(typeof(IDomainEventHub<>),
            typeof(DomainEventHub<>));
        return services;
    }
}