namespace Microsoft.AspNetCore.Builder;

using NorthWind.Sales.Backend.DataContexts.EFCore.Services;

public static class EndpointsContainer
{
    public static WebApplication MapNorthwindSalesEndpoints(
        this WebApplication app)
    {
        app.UseCreateOrderController();
        app.UseCustomerQueryController();
        app.UseProductQueryController();
        app.UseOrderQueryController();
        app.UseMembershipEndpoints();

        // Initialize database if needed
        Task.Run(async () => await DatabaseInitializer.InitializeDatabaseAsync(app.Services));

        return app;
    }
}