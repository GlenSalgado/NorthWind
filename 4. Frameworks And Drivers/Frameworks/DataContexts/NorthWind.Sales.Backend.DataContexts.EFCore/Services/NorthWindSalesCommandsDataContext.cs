namespace NorthWind.Sales.Backend.DataContexts.EFCore.Services;

using Microsoft.Extensions.Options;
using NorthWind.Sales.Backend.BusinessObjects.POCOEntities;
using NorthWind.Sales.Backend.DataContexts.EFCore.DataContexts;
using NorthWind.Sales.Backend.DataContexts.EFCore.Guards;
using NorthWind.Sales.Backend.DataContexts.EFCore.Options;
using NorthWind.Sales.Backend.Repositories.Entities;
using NorthWind.Sales.Backend.Repositories.Interfaces;

internal class NorthWindSalesCommandsDataContext(IOptions<DBOptions> dbOptions)
    : NorthWindSalesContext(dbOptions), INorthWindSalesCommandsDataContext
{
    public async Task AddOrderAsync(Order order) =>
        await AddAsync(order);

    public async Task AddOrderDetailsAsync(
        IEnumerable<NorthWind.Sales.Backend.Repositories.Entities.OrderDetail> orderDetails) =>
        await AddRangeAsync(orderDetails);

    public async Task DecreaseProductsStockAsync(
        IEnumerable<(int ProductId, short Quantity)> items)
    {
        foreach (var (productId, quantity) in items)
        {
            var product = await Products.FindAsync(productId);
            if (product != null)
            {
                product.UnitsInStock = (short)Math.Max(0, product.UnitsInStock - quantity);
            }
        }
    }

    public async Task SaveChangesAsync() =>
        await GuardDBContext.AgainstSaveChangesErrorAsync(this);
}