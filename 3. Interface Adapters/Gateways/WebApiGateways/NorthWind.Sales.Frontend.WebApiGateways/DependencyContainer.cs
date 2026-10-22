namespace Microsoft.Extensions.DependencyInjection;

using NorthWind.HttpDelegatingHandlers;
using NorthWind.Sales.Frontend.BusinessObjects.Interfaces;
using NorthWind.Sales.Frontend.WebApiGateways;

public static class DependencyContainer
{
    public static IServiceCollection AddWebApiGateways(
        this IServiceCollection services,
        Action<HttpClient> configureHttpClient,
        Action<IHttpClientBuilder> configureHttpClientBuilder)
    {
        services.AddExceptionDelegatingHandler();

        var createOrderBuilder = services
            .AddHttpClient<ICreateOrderGateway, CreateOrderGateway>(configureHttpClient)
            .AddHttpMessageHandler<ExceptionDelegatingHandler>();
        configureHttpClientBuilder(createOrderBuilder);

        var customerBuilder = services
            .AddHttpClient<ICustomerGateway, CustomerGateway>(configureHttpClient)
            .AddHttpMessageHandler<ExceptionDelegatingHandler>();
        configureHttpClientBuilder(customerBuilder);

        var productBuilder = services
            .AddHttpClient<IProductGateway, ProductGateway>(configureHttpClient)
            .AddHttpMessageHandler<ExceptionDelegatingHandler>();
        configureHttpClientBuilder(productBuilder);

        var orderQueryBuilder = services
            .AddHttpClient<IOrderQueryGateway, OrderQueryGateway>(configureHttpClient)
            .AddHttpMessageHandler<ExceptionDelegatingHandler>();
        configureHttpClientBuilder(orderQueryBuilder);

        return services;
    }
}