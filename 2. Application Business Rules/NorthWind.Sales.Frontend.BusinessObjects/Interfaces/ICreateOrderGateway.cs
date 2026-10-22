namespace NorthWind.Sales.Frontend.BusinessObjects.Interfaces;

using NorthWind.Sales.Entities.Dtos.CreateOrder;

public interface ICreateOrderGateway
{
    Task<int> CreateOrderAsync(CreateOrderDto order);
}