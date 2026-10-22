namespace Microsoft.Extensions.DependencyInjection;

using NorthWind.Sales.Entities.Dtos.CreateOrder;
using NorthWind.Sales.Validators.Entities.CreateOrder;

public static class DependencyContainer
{
    public static IServiceCollection AddValidators(
        this IServiceCollection services)
    {
        services.AddModelValidator<CreateOrderDto,
            CreateOrderDtoValidator>();
        services.AddModelValidator<CreateOrderDetailDto,
            CreateOrderDetailDtoValidator>();
        return services;
    }
}