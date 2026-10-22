namespace NorthWind.Sales.Backend.DataContexts.EFCore.Services;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public static class DatabaseInitializer
{
    public static async Task InitializeDatabaseAsync(IServiceProvider serviceProvider)
    {
        try
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetService<NorthWindSalesCommandsDataContext>();
            if (context != null)
            {
                await context.Database.EnsureCreatedAsync();

                // Ensure Cedula and Email columns exist on Customers table in case database pre-existed
                try
                {
                    await context.Database.ExecuteSqlRawAsync(@"
                        IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'Customers') AND name = 'Cedula')
                        BEGIN
                            ALTER TABLE Customers ADD Cedula NVARCHAR(10) NULL;
                        END
                    ");

                    await context.Database.ExecuteSqlRawAsync(@"
                        IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'Customers') AND name = 'Email')
                        BEGIN
                            ALTER TABLE Customers ADD Email NVARCHAR(100) NULL;
                        END
                    ");

                    await context.Database.ExecuteSqlRawAsync(@"
                        UPDATE Customers SET Cedula = '1851076057', Email = 'alfreds@northwind.com.ec' WHERE Id = 'ALFKI' AND (Cedula IS NULL OR Cedula = '');
                        UPDATE Customers SET Cedula = '1710034065', Email = 'ana.trujillo@northwind.com.ec' WHERE Id = 'ANATR' AND (Cedula IS NULL OR Cedula = '');
                        UPDATE Customers SET Cedula = '0102030400', Email = 'antonio.moreno@northwind.com.ec' WHERE Id = 'ANTON' AND (Cedula IS NULL OR Cedula = '');
                        UPDATE Customers SET Cedula = '1801234566', Email = 'around.horn@northwind.com.ec' WHERE Id = 'AROUT' AND (Cedula IS NULL OR Cedula = '');
                        UPDATE Customers SET Cedula = '1802345676', Email = 'berglunds@northwind.com.ec' WHERE Id = 'BERGS' AND (Cedula IS NULL OR Cedula = '');
                        UPDATE Customers SET Cedula = '1803456787', Email = 'blauer.see@northwind.com.ec' WHERE Id = 'BLAUS' AND (Cedula IS NULL OR Cedula = '');
                        UPDATE Customers SET Cedula = '1804567897', Email = 'blondel@northwind.com.ec' WHERE Id = 'BLONP' AND (Cedula IS NULL OR Cedula = '');
                        UPDATE Customers SET Cedula = '1805678909', Email = 'bolido@northwind.com.ec' WHERE Id = 'BOLID' AND (Cedula IS NULL OR Cedula = '');
                        UPDATE Customers SET Cedula = '1806789010', Email = 'bonapp@northwind.com.ec' WHERE Id = 'BONAP' AND (Cedula IS NULL OR Cedula = '');
                        UPDATE Customers SET Cedula = '1807890122', Email = 'bottom.dollar@northwind.com.ec' WHERE Id = 'BOTTM' AND (Cedula IS NULL OR Cedula = '');
                    ");
                }
                catch
                {
                    // Ignore column check failures if not supported
                }
            }
        }
        catch
        {
            // If already created or localdb unavailable, allow app to proceed
        }
    }
}
