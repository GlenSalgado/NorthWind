namespace Microsoft.Extensions.DependencyInjection;

using NorthWind.Transactions.Entities.Interfaces;
using NorthWind.Transactions.Entities.Services;

public static class DependencyContainer
{
    public static IServiceCollection AddTransactionServices(
        this IServiceCollection services)
    {
        services.AddTransient<IDomainTransaction, DomainTransaction>();
        return services;
    }
}