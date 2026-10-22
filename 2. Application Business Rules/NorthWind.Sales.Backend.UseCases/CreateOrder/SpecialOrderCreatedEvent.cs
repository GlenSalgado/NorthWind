namespace NorthWind.Sales.Backend.UseCases.CreateOrder;

using NorthWind.Events.Entities.Interfaces;

internal class SpecialOrderCreatedEvent(
    int orderId, int productsCount) : IDomainEvent
{
    public int OrderId => orderId;
    public int ProductsCount => productsCount;
}