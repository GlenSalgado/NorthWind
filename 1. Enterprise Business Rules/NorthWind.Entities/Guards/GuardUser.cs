namespace NorthWind.Entities.Guards;

using NorthWind.Entities.Interfaces;

public static class GuardUser
{
    public static void AgainstUnauthenticated(IUserService userService)
    {
        if (!userService.IsAuthenticated)
            throw new UnauthorizedAccessException();
    }
}