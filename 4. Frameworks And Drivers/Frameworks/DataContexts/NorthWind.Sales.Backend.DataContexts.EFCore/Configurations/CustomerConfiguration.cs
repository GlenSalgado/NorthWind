namespace NorthWind.Sales.Backend.DataContexts.EFCore.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NorthWind.Sales.Backend.Repositories.Entities;

internal class CustomerConfiguration :
    IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder
            .Property(c => c.Id)
            .HasMaxLength(5)
            .IsFixedLength();

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(40);

        builder.Property(c => c.CurrentBalance)
            .HasPrecision(8, 2);

        builder.Property(c => c.Cedula)
            .HasMaxLength(10);

        builder.Property(c => c.Email)
            .HasMaxLength(100);

        builder.HasData(
            new Customer { Id = "ALFKI", Name = "Alfreds Futterkiste", CurrentBalance = 0, Cedula = "1851076057", Email = "alfreds@northwind.com.ec" },
            new Customer { Id = "ANATR", Name = "Ana Trujillo Emparedados y helados", CurrentBalance = 0, Cedula = "1710034065", Email = "ana.trujillo@northwind.com.ec" },
            new Customer { Id = "ANTON", Name = "Antonio Moreno Taquería", CurrentBalance = 100, Cedula = "0102030400", Email = "antonio.moreno@northwind.com.ec" },
            new Customer { Id = "AROUT", Name = "Around the Horn", CurrentBalance = 50, Cedula = "1801234566", Email = "around.horn@northwind.com.ec" },
            new Customer { Id = "BERGS", Name = "Berglunds snabbköp", CurrentBalance = 0, Cedula = "1802345676", Email = "berglunds@northwind.com.ec" },
            new Customer { Id = "BLAUS", Name = "Blauer See Delikatessen", CurrentBalance = 0, Cedula = "1803456787", Email = "blauer.see@northwind.com.ec" },
            new Customer { Id = "BLONP", Name = "Blondel père et fils", CurrentBalance = 240, Cedula = "1804567897", Email = "blondel@northwind.com.ec" },
            new Customer { Id = "BOLID", Name = "Bólido Comidas preparadas", CurrentBalance = 0, Cedula = "1805678909", Email = "bolido@northwind.com.ec" },
            new Customer { Id = "BONAP", Name = "Bon app'", CurrentBalance = 0, Cedula = "1806789010", Email = "bonapp@northwind.com.ec" },
            new Customer { Id = "BOTTM", Name = "Bottom-Dollar Markets", CurrentBalance = 15, Cedula = "1807890122", Email = "bottom.dollar@northwind.com.ec" }
        );
    }
}