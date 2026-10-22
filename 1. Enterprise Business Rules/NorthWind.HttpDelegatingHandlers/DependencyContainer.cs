namespace Microsoft.Extensions.DependencyInjection;

using Microsoft.Extensions.DependencyInjection.Extensions;
using NorthWind.HttpDelegatingHandlers;

public static class DependencyContainer
{
    public static IServiceCollection AddExceptionDelegatingHandler(
        this IServiceCollection services)
    {
        services.TryAddTransient<ExceptionDelegatingHandler>();
        return services;
    }
}