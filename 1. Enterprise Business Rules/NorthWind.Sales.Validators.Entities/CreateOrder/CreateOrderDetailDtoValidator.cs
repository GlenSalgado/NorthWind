namespace NorthWind.Sales.Validators.Entities.CreateOrder;

using NorthWind.Sales.Entities.Dtos.CreateOrder;
using NorthWind.Sales.Validators.Entities.Resources;
using NorthWind.Validation.Entities.Abstractions;
using NorthWind.Validation.Entities.Interfaces;

internal class CreateOrderDetailDtoValidator :
    AbstractModelValidator<CreateOrderDetailDto>
{
    public CreateOrderDetailDtoValidator(
        IValidationService<CreateOrderDetailDto> validationService) : base(validationService)
    {
        AddRuleFor(d => d.ProductId)
            .GreaterThan(0, CreateOrderMessages.ProductIdGreaterThanZero);

        AddRuleFor(d => d.Quantity)
            .GreaterThan<short>(0, CreateOrderMessages.QuantityGreaterThanZero);

        AddRuleFor(d => d.UnitPrice)
            .GreaterThan<decimal>(0, CreateOrderMessages.UnitPriceGreaterThanZero);
    }
}