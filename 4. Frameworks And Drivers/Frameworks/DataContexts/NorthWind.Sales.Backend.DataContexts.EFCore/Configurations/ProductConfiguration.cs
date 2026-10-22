namespace NorthWind.Sales.Backend.DataContexts.EFCore.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NorthWind.Sales.Backend.Repositories.Entities;

internal class ProductConfiguration :
    IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(40);

        builder.Property(p => p.UnitPrice)
            .HasPrecision(8, 2);

        builder.HasData(
            new Product { Id = 1, Name = "Chai", UnitPrice = 18.00m, UnitsInStock = 39 },
            new Product { Id = 2, Name = "Chang (Agotado)", UnitPrice = 19.00m, UnitsInStock = 0 }, // Stock 0 for testing filter
            new Product { Id = 3, Name = "Aniseed Syrup", UnitPrice = 10.00m, UnitsInStock = 13 },
            new Product { Id = 4, Name = "Chef Anton's Cajun Seasoning", UnitPrice = 22.00m, UnitsInStock = 53 },
            new Product { Id = 5, Name = "Chef Anton's Gumbo Mix (Agotado)", UnitPrice = 21.35m, UnitsInStock = 0 }, // Stock 0 for testing filter
            new Product { Id = 6, Name = "Grandma's Boysenberry Spread", UnitPrice = 25.00m, UnitsInStock = 120 },
            new Product { Id = 7, Name = "Uncle Bob's Organic Dried Pears", UnitPrice = 30.00m, UnitsInStock = 15 },
            new Product { Id = 8, Name = "Northwoods Cranberry Sauce", UnitPrice = 40.00m, UnitsInStock = 6 },
            new Product { Id = 9, Name = "Mishi Kobe Niku", UnitPrice = 97.00m, UnitsInStock = 29 },
            new Product { Id = 10, Name = "Ikura", UnitPrice = 31.00m, UnitsInStock = 31 },
            new Product { Id = 11, Name = "Queso Cabrales", UnitPrice = 21.00m, UnitsInStock = 22 },
            new Product { Id = 12, Name = "Queso Manchego La Pastora", UnitPrice = 38.00m, UnitsInStock = 86 },
            new Product { Id = 13, Name = "Konbu", UnitPrice = 6.00m, UnitsInStock = 24 },
            new Product { Id = 14, Name = "Tofu", UnitPrice = 23.25m, UnitsInStock = 35 },
            new Product { Id = 15, Name = "Genen Shouyu", UnitPrice = 15.50m, UnitsInStock = 39 }
        );
    }
}