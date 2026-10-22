namespace Microsoft.Extensions.DependencyInjection;

using NorthWind.Entities.Interfaces;
using NorthWind.UserServices;

public static class DependencyContainer
{
    public static IServiceCollection AddUserServices(
        this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddSingleton<IUserService, UserService>();
        return services;
    }
}