namespace NorthWind.UserServices;

using NorthWind.Entities.Interfaces;

internal class UserServiceFake : IUserService
{
    public bool IsAuthenticated => true;
    public string UserName => "user@northwind.com";
    public string FullName => "Usuario de prueba";
}