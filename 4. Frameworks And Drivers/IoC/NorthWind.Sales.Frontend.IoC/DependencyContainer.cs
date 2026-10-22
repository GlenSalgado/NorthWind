namespace Microsoft.Extensions.DependencyInjection;

using NorthWind.Sales.Frontend.WebApiGateways;
using NorthWind.Sales.Frontend.Views;

public static class DependencyContainer
{
    public static IServiceCollection AddNorthWindSalesServices(
        this IServiceCollection services,
        Action<HttpClient> configureHttpClient,
        Action<HttpClient> configureMembershipHttpClient,
        Action<IHttpClientBuilder> configureHttpClientBuilder)
    {
        services.AddWebApiGateways(configureHttpClient, configureHttpClientBuilder)
            .AddViewsServices()
            .AddValidationService()
            .AddValidators()
            .AddMembershipServices(configureMembershipHttpClient);
        return services;
    }
}