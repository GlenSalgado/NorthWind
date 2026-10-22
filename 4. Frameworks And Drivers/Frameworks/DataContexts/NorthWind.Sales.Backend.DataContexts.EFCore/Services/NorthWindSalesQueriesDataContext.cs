namespace NorthWind.Sales.Backend.DataContexts.EFCore.Services;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NorthWind.Sales.Backend.BusinessObjects.POCOEntities;
using NorthWind.Sales.Backend.DataContexts.EFCore.DataContexts;
using NorthWind.Sales.Backend.DataContexts.EFCore.Options;
using NorthWind.Sales.Backend.Repositories.Entities;
using NorthWind.Sales.Backend.Repositories.Interfaces;

internal class NorthWindSalesQueriesDataContext :
    NorthWindSalesContext,
    INorthWindSalesQueriesDataContext
{
    public NorthWindSalesQueriesDataContext(IOptions<DBOptions> dbOptions)
        : base(dbOptions)
    {
        ChangeTracker.QueryTrackingBehavior =
            QueryTrackingBehavior.NoTracking;
    }

    public new IQueryable<Customer> Customers => base.Customers;
    public new IQueryable<Product> Products => base.Products;
    public new IQueryable<Order> Orders => base.Orders;
    public new IQueryable<NorthWind.Sales.Backend.Repositories.Entities.OrderDetail> OrderDetails => base.OrderDetails;

    public async Task<ReturnType> FirstOrDefaultAsync<ReturnType>(
        IQueryable<ReturnType> queryable) =>
        (await queryable.FirstOrDefaultAsync())!;

    public async Task<IEnumerable<ReturnType>> ToListAsync<ReturnType>(
        IQueryable<ReturnType> queryable) =>
        await queryable.ToListAsync();

    public async Task<int> CountAsync<ReturnType>(
        IQueryable<ReturnType> queryable) =>
        await queryable.CountAsync();
}