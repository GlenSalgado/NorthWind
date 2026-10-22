namespace Microsoft.Extensions.DependencyInjection;

using NorthWind.Sales.Backend.UseCases.CreateOrder;
using NorthWind.Sales.Entities.Dtos.CreateOrder;

public static class DependencyContainer
{
    public static IServiceCollection AddUseCasesServices(
        this IServiceCollection services)
    {
        services.AddScoped<ICreateOrderInputPort,
            CreateOrderInteractor>();

        services.AddModelValidator<CreateOrderDto,
            CreateOrderCustomerValidator>();
        services.AddModelValidator<CreateOrderDto,
            CreateOrderProductValidator>();

        services.AddScoped<IDomainEventHandler<SpecialOrderCreatedEvent>,
            SendEmailWhenSpecialOrderCreatedEventHandler>();

        return services;
    }
}