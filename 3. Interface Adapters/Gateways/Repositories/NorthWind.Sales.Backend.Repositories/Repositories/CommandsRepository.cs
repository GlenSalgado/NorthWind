namespace NorthWind.Sales.Backend.Repositories.Repositories;

internal class CommandsRepository(
    INorthWindSalesCommandsDataContext context)
    : ICommandsRepository
{
    public async Task CreateOrder(OrderAggregate order)
    {
        await context.AddOrderAsync(order);
        await context.AddOrderDetailsAsync(
            order.OrderDetails
            .Select(d => new Entities.OrderDetail
            {
                Order = order,
                ProductId = d.ProductId,
                Quantity = d.Quantity,
                UnitPrice = d.UnitPrice
            }).ToArray());

        // Requirement 7: Decrease stock automatically in products table
        await context.DecreaseProductsStockAsync(
            order.OrderDetails.Select(d => (d.ProductId, d.Quantity)));
    }

    public async Task SaveChanges() =>
        await context.SaveChangesAsync();
}