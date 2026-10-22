namespace NorthWind.Sales.Validators.Entities.CreateOrder;

using NorthWind.Sales.Entities.Dtos.CreateOrder;
using NorthWind.Sales.Validators.Entities.Resources;
using NorthWind.Sales.Entities.Validators;
using NorthWind.Validation.Entities.Abstractions;
using NorthWind.Validation.Entities.Interfaces;

internal class CreateOrderDtoValidator :
    AbstractModelValidator<CreateOrderDto>
{
    public CreateOrderDtoValidator(
        IValidationService<CreateOrderDto> validationService,
        IModelValidator<CreateOrderDetailDto> detailValidator)
        : base(validationService)
    {
        AddRuleFor(c => c.CustomerId)
            .NotEmpty(CreateOrderMessages.CustomerIdRequired)
            .Length(5, CreateOrderMessages.CustomerIdRequiredLength);

        AddRuleFor(c => c.ShipAddress)
            .NotEmpty(CreateOrderMessages.ShipAddressRequired)
            .MaximumLength(60, CreateOrderMessages.ShipAddressMaximumLength)
            .Must(value => string.IsNullOrWhiteSpace(value) || ValidationRulesHelper.IsValidAddress(value), "La dirección contiene caracteres no permitidos.");

        AddRuleFor(c => c.ShipCity)
            .NotEmpty(CreateOrderMessages.ShipCityRequired)
            .MinimumLength(3, CreateOrderMessages.ShipCityMinimumLength)
            .MaximumLength(15, CreateOrderMessages.ShipCityMaximumLength)
            .Must(ValidationRulesHelper.IsOnlyLetters, "La ciudad solo puede contener letras y espacios.");

        AddRuleFor(c => c.ShipCountry)
            .NotEmpty(CreateOrderMessages.ShipCountryRequired)
            .MinimumLength(3, CreateOrderMessages.ShipCountryMinimumLength)
            .MaximumLength(15, CreateOrderMessages.ShipCountryMaximumLength)
            .Must(ValidationRulesHelper.IsOnlyLetters, "El país solo puede contener letras y espacios.");

        AddRuleFor(c => c.ShipPostalCode)
            .MaximumLength(10, CreateOrderMessages.ShipPostalCodeMaximumLength)
            .Must(value => string.IsNullOrWhiteSpace(value) || ValidationRulesHelper.IsOnlyNumbers(value), "El código postal solo puede contener números.");

        AddRuleFor(c => c.OrderDetails)
            .NotNull(CreateOrderMessages.OrderDetailsRequired)
            .NotEmpty(CreateOrderMessages.OrderDetailsNotEmpty);

        AddRuleForEach(c => c.OrderDetails)
            .SetValidator(detailValidator);
    }
}
