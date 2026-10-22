using NorthWind.Sales.Backend.BusinessObjects.POCOEntities;

namespace NorthWind.Sales.Backend.Repositories.Interfaces;

public interface INorthWindSalesCommandsDataContext
{
    Task AddOrderAsync(Order order);
    Task AddOrderDetailsAsync(IEnumerable<Entities.OrderDetail> orderDetails);
    Task DecreaseProductsStockAsync(IEnumerable<(int ProductId, short Quantity)> items);
    Task SaveChangesAsync();
}