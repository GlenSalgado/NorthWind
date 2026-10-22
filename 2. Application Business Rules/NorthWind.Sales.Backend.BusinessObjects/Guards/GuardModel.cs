namespace NorthWind.Sales.Backend.BusinessObjects.Guards;

using NorthWind.Exceptions.Entities.Exceptions;
using NorthWind.Validation.Entities.Interfaces;

public static class GuardModel
{
    public static async Task AgainstNotValid<T>(
        IModelValidatorHub<T> modelValidatorHub, T model)
    {
        if (!await modelValidatorHub.Validate(model))
            throw new ValidationException(modelValidatorHub.Errors);
    }
}