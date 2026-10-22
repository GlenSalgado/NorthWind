namespace NorthWind.Sales.Backend.DataContexts.EFCore.DataContexts;

using Microsoft.EntityFrameworkCore;
using NorthWind.Sales.Backend.BusinessObjects.POCOEntities;
using NorthWind.Sales.Backend.Repositories.Entities;
using System.Reflection;

internal class NorthWindContext : DbContext
{
    protected override void OnConfiguring(
        DbContextOptionsBuilder optionsBuilder)
    {
        string connectionString =
            Environment.GetEnvironmentVariable("NORTHWIND_DB_CONNECTION")
            ?? throw new InvalidOperationException(
                "Set NORTHWIND_DB_CONNECTION before running NorthWind EF design-time commands.");
        optionsBuilder.UseSqlServer(connectionString);
    }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<NorthWind.Sales.Backend.Repositories.Entities.OrderDetail> OrderDetails { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            Assembly.GetExecutingAssembly());
    }
}
