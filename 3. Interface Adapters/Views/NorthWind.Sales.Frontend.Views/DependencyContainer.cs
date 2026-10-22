namespace Microsoft.Extensions.DependencyInjection;

using NorthWind.Sales.Frontend.Views.ViewModels.CreateOrder;
using NorthWind.Sales.Frontend.Views.ViewModels.Invoices;

public static class DependencyContainer
{
    public static IServiceCollection AddViewsServices(
        this IServiceCollection services)
    {
        services.AddScoped<CreateOrderViewModel>();
        services.AddScoped<InvoiceViewerViewModel>();
        services.AddModelValidator<CreateOrderViewModel,
            CreateOrderViewModelValidator>();
        return services;
    }
}