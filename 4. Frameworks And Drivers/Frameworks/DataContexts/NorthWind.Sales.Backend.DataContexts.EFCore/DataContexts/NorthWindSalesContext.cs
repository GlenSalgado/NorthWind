namespace NorthWind.Sales.Backend.DataContexts.EFCore.DataContexts;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NorthWind.Sales.Backend.BusinessObjects.POCOEntities;
using NorthWind.Sales.Backend.DataContexts.EFCore.Options;
using NorthWind.Sales.Backend.Repositories.Entities;
using System.Reflection;

internal class NorthWindSalesContext(IOptions<DBOptions> dbOptions) : DbContext
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<NorthWind.Sales.Backend.Repositories.Entities.OrderDetail> OrderDetails { get; set; }

    protected override void OnConfiguring(
        DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            dbOptions.Value.ConnectionString);
    }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            Assembly.GetExecutingAssembly());
    }
}